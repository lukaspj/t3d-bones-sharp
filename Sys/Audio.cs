using System;
using System.Collections.Generic;
using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Sys
{
   class Audio
   {
      private static SFXSource AudioChannelMaster;
      private static readonly List<SFXSource> AudioChannels = new List<SFXSource>();
      private static SimSet SFXPausedSet;

      public static void Init()
      {
         //-----------------------------------------------------------------------------
         //    Source groups.
         //-----------------------------------------------------------------------------
         SFXDescription AudioMaster = new SFXDescription("AudioMaster");
         AudioMaster.registerObject();
         Console.WriteLine(AudioMaster.ConeInsideAngle);

         AudioChannelMaster = new SFXSource("AudioChannelMaster")
         {
            Description = AudioMaster
         };
         AudioChannelMaster.registerObject();
         Console.WriteLine(AudioChannelMaster.Description.getName());

         SFXDescription AudioChannel = new SFXDescription("AudioChannel")
         {
            SourceGroup = AudioChannelMaster
         };
         AudioChannel.registerObject();

         SFXSource AudioChannelDefault = new SFXSource("AudioChannelDefault")
         {
            Description = AudioChannel
         };
         AudioChannelDefault.registerObject();

         SFXSource AudioChannelGui = new SFXSource("AudioChannelGui")
         {
            Description = AudioChannel
         };
         AudioChannelGui.registerObject();

         SFXSource AudioChannelEffects = new SFXSource("AudioChannelEffects")
         {
            Description = AudioChannel
         };
         AudioChannelEffects.registerObject();

         SFXSource AudioChannelMessages = new SFXSource("AudioChannelMessages")
         {
            Description = AudioChannel
         };
         AudioChannelMessages.registerObject();

         SFXSource AudioChannelMusic = new SFXSource("AudioChannelMusic")
         {
            Description = AudioChannel
         };
         AudioChannelMusic.registerObject();

         // Set default playback states of the channels.

         AudioChannelMaster.play();
         AudioChannelDefault.play();

         AudioChannelGui.play();
         AudioChannelMusic.play();
         AudioChannelMessages.play();

         // Stop in-game effects channels.
         AudioChannelEffects.stop();

         AudioChannels.Add(AudioChannelDefault);
         AudioChannels.Add(AudioChannelGui);
         AudioChannels.Add(AudioChannelEffects);
         AudioChannels.Add(AudioChannelMessages);
         AudioChannels.Add(AudioChannelMusic);

         //-----------------------------------------------------------------------------
         //    Master SFXDescriptions.
         //-----------------------------------------------------------------------------
         // Master description for interface audio.
         SFXDescription AudioGui = new SFXDescription("AudioGui")
         {
            Volume = 1.0f,
            SourceGroup = AudioChannelGui
         };
         AudioGui.registerObject();

         // Master description for game effects audio.
         SFXDescription AudioEffect = new SFXDescription("AudioEffect")
         {
            Volume = 1.0f,
            SourceGroup = AudioChannelEffects
         };
         AudioEffect.registerObject();

         // Master description for audio in notifications.
         SFXDescription AudioMessage = new SFXDescription("AudioMessage")
         {
            Volume = 1.0f,
            SourceGroup = AudioChannelMessages
         };
         AudioMessage.registerObject();

         // Master description for music.
         SFXDescription AudioMusic = new SFXDescription("AudioMusic")
         {
            Volume = 1.0f,
            SourceGroup = AudioChannelMusic
         };
         AudioMusic.registerObject();

         SFXPausedSet = new SimSet("SFXPausedSet");
         SFXPausedSet.registerObject();

         InitBackwardsCompatibility();
      }

      //-----------------------------------------------------------------------------
      //    SFX Functions.
      //-----------------------------------------------------------------------------

      /// This initializes the sound system device from
      /// the defaults in the $pref::SFX:: globals.
      public static void sfxStartup()
      {
         Global.echo("\nsfxStartup...");

         // If we have a provider set, try initialize a device now.

         if (!string.IsNullOrEmpty(Globals.GetString("pref::SFX::provider")))
         {
            if (sfxInit())
               return;

            // Force auto-detection.
            Globals.SetBool("pref::SFX::autoDetect", true);
         }

         // If enabled autodetect a safe device.

         if ((!Global.isDefined("$pref::SFX::autoDetect") || Globals.GetBool("pref::SFX::autoDetect")) &&
             sfxAutodetect())
            return;

         // Failure.

         Global.error("   Failed to initialize device!\n\n");

         Globals.SetString("pref::SFX::provider", "");
         Globals.SetString("pref::SFX::device", "");
      }

      /// This initializes the sound system device from
      /// the defaults in the $pref::SFX:: globals.
      public static bool sfxInit()
      {
         // If already initialized, shut down the current device first.

         if (!string.IsNullOrEmpty(Global.sfxGetDeviceInfo()))
            sfxShutdown();

         // Start it up!
         Globals.SetInt("maxBuffers",
            Globals.GetBool("pref::SFX::useHardware") ? -1 : Globals.GetInt("pref::SFX::maxSoftwareBuffers"));
         if (
            !Global.sfxCreateDevice(Globals.GetString("pref::SFX::provider"), Globals.GetString("pref::SFX::device"),
               Globals.GetBool("pref::SFX::useHardware"), Globals.GetInt("maxBuffers")))
            return false;

         // This returns a tab seperated string with
         // the initialized system info.
         string info = Global.sfxGetDeviceInfo();
         Globals.SetString("pref::SFX::provider", Global.getField(info, 0));
         Globals.SetString("pref::SFX::device", Global.getField(info, 1));
         Globals.SetString("pref::SFX::useHardware", Global.getField(info, 2));
         string useHardware = Globals.GetBool("pref::SFX::useHardware") ? "Yes" : "No";
         string maxBuffers = Global.getField(info, 3);

         Global.echo("   Provider: " + Globals.GetString("pref::SFX::provider"));
         Global.echo("   Device: " + Globals.GetString("pref::SFX::device"));
         Global.echo("   Hardware: " + useHardware);
         Global.echo("   Buffers: " + maxBuffers);
         Global.echo(" ");

         if (Global.isDefined("$pref::SFX::distanceModel"))
            Global.sfxSetDistanceModel(
               (SFXDistanceModel) Enum.Parse(typeof(SFXDistanceModel), Globals.GetString("pref::SFX::distanceModel")));
         if (Global.isDefined("$pref::SFX::dopplerFactor"))
            Global.sfxSetDopplerFactor(Globals.GetFloat("pref::SFX::dopplerFactor"));
         if (Global.isDefined("$pref::SFX::rolloffFactor"))
            Global.sfxSetRolloffFactor(Globals.GetFloat("pref::SFX::rolloffFactor"));

         // Restore master volume.

         sfxSetMasterVolume(Globals.GetFloat("pref::SFX::masterVolume"));

         // Restore channel volumes.

         for (int index = 0; index < AudioChannels.Count; index++)
         {
            sfxSetChannelVolume(index, Globals.GetFloat($"pref::SFX::channelVolume[{index}]"));
         }

         return true;
      }

      /// Destroys the current sound system device.
      public static void sfxShutdown()
      {
         // Store volume prefs.

         Globals.SetFloat("pref::SFX::masterVolume", sfxGetMasterVolume());

         for (int channel = 0; channel <= 8; channel++)
         {
            Globals.SetFloat($"pref::SFX::channelVolume[{channel}]", sfxGetChannelVolume(channel));
         }

         // We're assuming here that a null info 
         // string means that no device is loaded.
         if (string.IsNullOrEmpty(Global.sfxGetDeviceInfo()))
            return;

         Global.sfxDeleteDevice();
      }

      /// Determines which of the two SFX providers is preferable.
      public static int sfxCompareProvider(string providerA, string providerB)
      {
         if (providerA.Equals(providerB))
            return 0;

         switch (providerA)
         {
            // Always prefer FMOD over anything else.
            case "FMOD":
               return 1;

            // Prefer OpenAL over anything but FMOD.
            case "OpenAL":
               if (providerB.Equals("FMOD"))
                  return -1;
               return 1;
            // As long as the XAudio SFX provider still has issues,
            // choose stable DSound over it.
            case "DirectSound":
               if (providerB.Equals("FMOD") || providerB.Equals("OpenAL"))
                  return -1;
               return 0;
            case "XAudio":
               if (!providerB.Equals("FMOD") && !providerB.Equals("OpenAL") && !providerB.Equals("DirectSound"))
                  return 1;
               return -1;
            default:
               return -1;
         }
      }

      /// Try to detect and initalize the best SFX device available.
      public static bool sfxAutodetect()
      {
         // Get all the available devices.
         string devices = Global.sfxGetAvailableDevices();

         // Collect and sort the devices by preferentiality.

         List<KeyValuePair<string, string>> deviceTrySequence = new List<KeyValuePair<string, string>>();
         int bestMatch = -1;
         int count = Global.getRecordCount(devices);
         for (int i = 0; i < count; i++)
         {
            string info = Global.getRecord(devices, i);
            string provider = Global.getField(info, 0);
            deviceTrySequence.Add(new KeyValuePair<string, string>(provider, info));
         }

         deviceTrySequence.Sort((a, b) => sfxCompareProvider(a.Key, b.Key));

         // Try the devices in order.

         count = deviceTrySequence.Count;

         for (int i = 0; i < count; i++)
         {
            string provider = deviceTrySequence[i].Key;
            string info = deviceTrySequence[i].Value;

            Globals.SetString("pref::SFX::provider", provider);
            Globals.SetString("pref::SFX::device", Global.getField(info, 1));
            Globals.SetString("pref::SFX::useHardware", Global.getField(info, 2));

            // By default we've decided to avoid hardware devices as
            // they are buggy and prone to problems.
            Globals.SetBool("pref::SFX::useHardware", false);

            if (sfxInit())
            {
               Globals.SetBool("pref::SFX::autoDetect", false);
               return true;
            }
         }

         // Found no suitable device.

         Global.error("sfxAutodetect - Could not initialize a valid SFX device.");

         Globals.SetString("pref::SFX::provider", "");
         Globals.SetString("pref::SFX::device", "");
         Globals.SetString("pref::SFX::useHardware", "");

         return false;
      }

      private static void sfxSetMasterVolume(float volume)
      {
         AudioChannelMaster.setVolume(volume);
      }

      private static float sfxGetMasterVolume()
      {
         return AudioChannelMaster.getVolume();
      }

      public static void sfxSetChannelVolume(int channel, float volume)
      {
         AudioChannels[channel].setVolume(volume);
      }

      public static float sfxGetChannelVolume(int channel)
      {
         return AudioChannels[channel].getVolume();
      }

      private static void sfxStopAll(int channel)
      {
         // Don't stop channel itself since that isn't quite what the function
         // here intends.

         SFXSource source = sfxOldChannelToGroup(channel);

         for (uint i = 0; i < source.getCount(); i++)
         {
            source.getObject(i).As<SFXSource>().stop();
         }
      }

      /// Pauses the playback of active sound sources.
      ///  
      /// @param %channels    An optional word list of channel indices or an empty 
      /// string to pause sources on all channels.
      /// @param %pauseSet    An optional SimSet which is filled with the paused 
      /// sources.  If not specified the global SFXPausedSet 
      /// is used.
      /// 
      /// @deprecated
      public static void sfxPause(List<int> channels = null, SimSet pauseSet = null)
      {
         // Did we get a set to populate?
         if (pauseSet == null)
            pauseSet = SFXPausedSet;

         SimSet SFXSourceSet = Sim.FindObjectByName<SimSet>("SFXSourceSet");

         int count = SFXSourceSet.getCount();
         for (uint i = 0; i < count; i++)
         {
            SFXSource source = SFXSourceSet.getObject(i).As<SFXSource>();

            int channel = sfxGroupToOldChannel(source.getGroup());
            if (channels != null && !channels.Contains(channel)) continue;

            source.pause();
            pauseSet.add(source);
         }
      }

      /// Resumes the playback of paused sound sources.
      /// 
      /// @param %pauseSet    An optional SimSet which contains the paused sound 
      ///                     sources to be resumed.  If not specified the global 
      ///                     SFXPausedSet is used.
      /// @deprecated
      ///
      public static void sfxResume( SimSet pauseSet = null )
      {
         if (pauseSet == null)
            pauseSet = SFXPausedSet;
         
         int count = pauseSet.getCount();
         for (uint i = 0; i < count; i++)
         {
            SFXSource source = pauseSet.getObject(i).As<SFXSource>();
            source.play();
         }

         // Clear our pause set... the caller is left
         // to clear his own if he passed one.
         pauseSet.clear();
      }

      //-----------------------------------------------------------------------------
      //    Backwards-compatibility with old channel system.
      //-----------------------------------------------------------------------------

      private static void InitBackwardsCompatibility()
      {
         // Volume channel IDs for backwards-compatibility.
         Globals.SetInt("GuiAudioType", 1); // Interface.
         Globals.SetInt("SimAudioType", 2); // Game.
         Globals.SetInt("MessageAudioType", 3); // Notifications.
         Globals.SetInt("MusicAudioType", 4); // Music.
      }

      private static SFXSource sfxOldChannelToGroup(int channel)
      {
         return AudioChannels[channel];
      }

      private static int sfxGroupToOldChannel(SimGroup group)
      {
         int id = group.getId();
         for (int index = 0; index < AudioChannels.Count; index++)
         {
            SFXSource audioChannel = AudioChannels[index];
            if (audioChannel.getId() == id)
            {
               return index;
            }
         }
         return -1;
      }
   }
}
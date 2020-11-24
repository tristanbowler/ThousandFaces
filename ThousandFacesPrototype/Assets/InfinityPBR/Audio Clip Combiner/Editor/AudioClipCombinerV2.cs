using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AudioClipCombiner
{
    public static class AudioClipCombinerV2
    {
        static float rescaleFactor = 32767; //to convert float to Int16

        const int HEADER_SIZE = 44;

        public static AudioClipScriptableObject audioClipScriptableObject;

        public static int TotalExports(AudioClipScriptableObject so)
        {
	        int totalExports	= 0;												
	        for(int n = 0; n < so.audioLayers.Count; n++)
	        {
		        if (totalExports == 0)
		        {
			        totalExports = so.audioLayers[n].clip.Length;
		        }
		        else if (so.audioLayers[n].clip.Length > 0)
		        {
			        totalExports = totalExports * so.audioLayers[n].clip.Length;
		        }
	        }

	        return totalExports;
        }

        public static bool AllLayersHaveClips(AudioClipScriptableObject so)
        {
	        for (int i = 0; i < so.audioLayers.Count; i++)
	        {
		        if (so.audioLayers[i].clip.Length == 0)
			        return false;
		        for (int c = 0; c < so.audioLayers[i].clip.Length; c++)
		        {
			        if (!so.audioLayers[i].clip[c])
				        return false;
		        }
	        }

	        return true;
        }
        
        public static void SaveNow()
	    {
			// Find total number of exports
			int totalExports	= 1;												// Start at 1...
			for(int n = 0; n < audioClipScriptableObject.audioLayers.Count; n++)
			{
				totalExports 	*= audioClipScriptableObject.audioLayers [n].clip.Length;						// Multiply by the number of clips in each layer
			}

			if (totalExports > 0) {
				float progressPercent	= 0.0f;
				int clipCount = 0;
				EditorUtility.DisplayProgressBar("Exporting Combined Audio Clips", "Clip " + clipCount + " of " + totalExports, progressPercent);
				string[] combinations;													// Start an array of all combinations
				combinations = new string[totalExports];								// Set the number of entries to the number of exports

				// Reset the onClip value for each layer
				for (int r = 0; r < audioClipScriptableObject.audioLayers.Count; r++) {
					audioClipScriptableObject.audioLayers [r].onClip = 0;
				}
					
				for (int l = 0; l < audioClipScriptableObject.audioLayers.Count; l++) {							// For each layer...
					int exportsLeft = 1;												// Start at 1...
					for (int i = l; i < audioClipScriptableObject.audioLayers.Count; i++) {							// For each layer left in the list (don't compute those we've already done)
						exportsLeft *= audioClipScriptableObject.audioLayers [i].clip.Length;					// Find out how many exports are left if it were just those layers
					}

					int entriesPerValue = exportsLeft / audioClipScriptableObject.audioLayers [l].clip.Length;	// Compute how many entires per value, if the total entries were exportsLeft
					int entryCount = 0;													// Set entryCount to 0

					for (int e = 0; e < combinations.Length; e++) {						// For all combinations
						if (l != 0)														// If this isn't the first layer
							combinations [e] = combinations [e] + ",";					// Append a "," to the String
						combinations [e] = combinations [e] + audioClipScriptableObject.audioLayers [l].onClip;	// Append the "onClip" value to the string
						entryCount++;													// increase entryCount
						if (entryCount >= entriesPerValue) {							// if we've done all the entires for that "onClip" value...
							audioClipScriptableObject.audioLayers [l].onClip++;									// increase onClip by 1
							entryCount = 0;												// Reset entryCount
							if (audioClipScriptableObject.audioLayers [l].onClip >= audioClipScriptableObject.audioLayers [l].clip.Length)	// if we've also run out of clips for this layer
								audioClipScriptableObject.audioLayers [l].onClip = 0;								// Reset onClip count
						}
					}
				}

				int overwriteProtectionNumber = 0;
				if (!audioClipScriptableObject.overwriteExports)
				{
					overwriteProtectionNumber = GetNumberOfLastClip();
				}

				int number = 0;															// for the file name
				// For each combination, save a .wav file with those clip numbers.
				foreach (var combination in combinations) {
					clipCount++;
					//progressPercent = clipCount / totalExports * 1.0f;
					progressPercent = clipCount / (float)totalExports;
					//Debug.Log ("progressPercent: " + progressPercent);
					EditorUtility.DisplayProgressBar("Exporting Combined Audio Clips", "Clip " + clipCount + " of " + totalExports, progressPercent);
					string[] clipsAsString	= combination.Split ("," [0]);
					SaveClip (audioClipScriptableObject.exportName, number + overwriteProtectionNumber, clipsAsString, audioClipScriptableObject.audioLayers);
					number++;
				}
			EditorUtility.ClearProgressBar();	
			} else {
				Debug.Log ("Nothing To Export! (or maybe a layer is missing clips?)");
			}
	    }

        public static int GetNumberOfLastClip()
        {
	        int lastClipNumber = 0;

	        //Debug.LogWarning("TO DO: Figure out why filePath is being denied");
	        
	        while (lastClipNumber < 10000)
	        {
		        string filePath = audioClipScriptableObject.exportPath + "/" + audioClipScriptableObject.exportName + "_" +
		                          lastClipNumber + ".wav";
		        filePath = filePath.Replace("Assets/Assets/", "Assets/");


		        //Debug.Log("FilePath: " + filePath);
		        if (!System.IO.File.Exists(filePath))
		        {
			        break;
		        }

		        lastClipNumber++;
	        }

	        //Debug.Log("Last Clip Numer: "+ lastClipNumber);
	        return lastClipNumber;
        }

		public static bool SaveClip(string filename, int exportNumber, string[] clipsAsString, List<AudioLayer> audioLayers)
		{
			if (filename.Length <= 0)															// If the name hasn't been set
				filename = "CombinedAudio" + exportNumber;										// Use a default name
			else {																				// else
				filename = filename + "_" + exportNumber;										// Use the chosen name plus the number
			}
			filename += ".wav";																	// add the .wav extension

			var filepath	= audioClipScriptableObject.exportPath + "/" + filename;			// Set the file path
			

			// Make sure directory exists if user is saving to sub dir.
			Directory.CreateDirectory(Path.GetDirectoryName(filepath));

			using (var fileStream = CreateEmpty(filepath))										// Create an empty file
			{
				int sampleCount = ConvertAndWrite(fileStream, clipsAsString, audioLayers);

				//	 ClIP NUMBER CHANGE HERE
				WriteHeader(fileStream, audioLayers[0].clip[0], sampleCount);
			}
			AssetDatabase.ImportAsset(filepath);
			return true; // TODO: return false if there's a failure saving the file
		}

		static int ConvertAndWrite(FileStream fileStream, String[] clipsAsString, List<AudioLayer> audioLayers)
	    {
	        int mostSamples = 0;																// Set this to 0
			//Debug.Log("audioLayers length: " + audioLayers.Count);
			for (int c = 0; c < audioLayers.Count; c++) {										// For each Layer
				int clipNumber = int.Parse(clipsAsString[c]);									// Get the clip number as an int
				audioLayers[c].GetSamples(clipNumber);											// Run this function from the class
				mostSamples = Mathf.Max(mostSamples, audioLayers[c].sampleCount);				// Set mostSamples to the greatest one
			}
			//Debug.Log("mostSamples: " + mostSamples);

	        
	        Int16[] finalSamples = new Int16[mostSamples];										// The exported clip will have the mostSamples
			float[] sampleValues = new float[mostSamples];

			int checkSampleCount = 0;
	        for(int i = 0; i < mostSamples; i++)												// for each sample
	        {
	            float sampleValue = 0;															// Set variable for exported clip
	            int sampleCount = 0;															// Set variable

	            foreach (var audioLayer in audioLayers)											// For each layer....
	            {
					if (i > audioLayer.delayCount && i < audioLayer.sampleCount)					// if we are not in the delay range and we are under the samplecount for the clip
	                {
						// Add the value from this layer to the final (sampleValue)
	                    sampleValue += (audioLayer.samples[i - audioLayer.delayCount] / rescaleFactor);
	                    sampleCount++;
	                }
	            }
	            
				sampleValues [i] += sampleValue;

				//if(sampleCount!=0)																// If we have done some samples (keep from dividing by 0)
	            //    sampleValue /= sampleCount;													// compute sampleValue
	            checkSampleCount++;
	        }
	        //Debug.Log("Did Sample Count: " + checkSampleCount);


			float highSample = 0.0f;																			// Variable for the highest sample
			float lowSample = 0.0f;																				// Variable for the lowest sample
			for (int h = 0; h < mostSamples; h++) {																// For each sample...
				highSample = Mathf.Max (highSample, sampleValues [h]);											// Compute the highest sample
				lowSample = Mathf.Min (lowSample, sampleValues [h]);    										// Compute the lowest sample
			}
			float parameter = Mathf.InverseLerp(0.0f, Mathf.Max(highSample, lowSample * -1), 1.0f);				// Find the amount we need to multiply each sample by, based on the most extreme sample (high or low)

			for (int p = 0; p < mostSamples; p++) {																// For each sample...
				sampleValues [p] *= parameter;																	// Multiply the value by the parameter value															// Adjust the volume
			}

			for (int i2 = 0; i2 < mostSamples; i2++) {															// For each sample...
				finalSamples [i2] = (short)(sampleValues[i2] * rescaleFactor);									// Finalize the value
			}
			sampleValues = new float[0];																		// Clear this data











			//Debug.Log("Final Samples: " + finalSamples.Length);


	        Byte[] bytesData = ConvertSamplesToBytes(finalSamples);

	        //Debug.Log("bytesData: " + bytesData.Length);
	        fileStream.Write(bytesData, 0, bytesData.Length);

	        return bytesData.Length;
	        //return mostSamples;
	    }


	    static Byte[] ConvertSamplesToBytes(Int16[] samples)
	    {
	        Byte[] bytesData = new Byte[samples.Length * 2];
	        for (int i = 0; i < samples.Length; i++)
	        {
	            Byte[] byteArr = new Byte[2];
	            byteArr = BitConverter.GetBytes(samples[i]);
	            byteArr.CopyTo(bytesData, i * 2);
	        }
	        return bytesData;
	    }


	    public static Int16[] GetSamplesFromClip(AudioClip clip, float volume = 1)
	    {
			//Debug.Log ("Getting Samples from clip " + clip.name);
	        var samples = new float[clip.samples * clip.channels];
			//Debug.Log ("Samples: " + samples.Length);

	        clip.GetData(samples, 0);

	        Int16[] intData = new Int16[samples.Length];
	        
			for (int i = 0; i < samples.Length; i++)
	        {
	            intData[i] = (short)(samples[i] * volume * rescaleFactor);
	        }
	        return intData;
	    }

	    

	    static void WriteHeader(FileStream fileStream, AudioClip clip, int sampleCount)
	    {
	        var frequency = clip.frequency;
	        var channelCount = clip.channels;

	        fileStream.Seek(0, SeekOrigin.Begin);

	        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
	        fileStream.Write(riff, 0, 4);

	        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
	        fileStream.Write(chunkSize, 0, 4);

	        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
	        fileStream.Write(wave, 0, 4);

	        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
	        fileStream.Write(fmt, 0, 4);

	        Byte[] subChunk1 = BitConverter.GetBytes(16);
	        fileStream.Write(subChunk1, 0, 4);

	        //UInt16 two = 2;
	        UInt16 one = 1;

	        Byte[] audioFormat = BitConverter.GetBytes(one);
	        fileStream.Write(audioFormat, 0, 2);

	        Byte[] numChannels = BitConverter.GetBytes(channelCount);
	        fileStream.Write(numChannels, 0, 2);

	        Byte[] sampleRate = BitConverter.GetBytes(frequency);
	        fileStream.Write(sampleRate, 0, 4);

	        Byte[] byteRate = BitConverter.GetBytes(frequency * channelCount * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
	        fileStream.Write(byteRate, 0, 4);

	        UInt16 blockAlign = (ushort)(channelCount * 2);
	        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

	        UInt16 bps = 16;
	        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
	        fileStream.Write(bitsPerSample, 0, 2);

	        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
	        fileStream.Write(datastring, 0, 4);

			//Byte[] subChunk2 = BitConverter.GetBytes(sampleCount * channelCount * 2);
	        Byte[] subChunk2 = BitConverter.GetBytes(sampleCount * channelCount * 1);
	        fileStream.Write(subChunk2, 0, 4);

	        //		fileStream.Close();
	    }

	    static FileStream CreateEmpty(string filepath)
	    {
	        var fileStream = new FileStream(filepath, FileMode.Create);
	        byte emptyByte = new byte();

	        for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
	        {
	            fileStream.WriteByte(emptyByte);
	        }

	        return fileStream;
	    }
    }
}


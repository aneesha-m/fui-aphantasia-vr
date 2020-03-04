﻿
//
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using UnityEngine.UI;


[RequireComponent (typeof (AudioSource))]

public class GoogleVoiceSpeech : MonoBehaviour {

		public Text TextBox;
        public GameObject terrain;
        public GameObject terrainContainer;
        public GameObject ocean;
        public GameObject table;
        public GameObject record;
        CanvasGroup canvasGroup;
        //public GameObject stop;
        public Text btntxt = null;

		struct ClipData
		{
				public int samples;
		}

		const int HEADER_SIZE = 44;

		private int minFreq;
		private int maxFreq;

		private bool micConnected = false;

		//A handle to the attached AudioSource
		private AudioSource goAudioSource;

		public string apiKey;

	// Use this for initialization
	void Start () {
                 //terrain.SetActive(false);
                 //ocean.SetActive(false);
                // table.SetActive(false);
				//stop.SetActive(false);
                //rend = GetComponent<SpriteRenderer>();
                //Color c = rend.material.color;
                //c.a = 0f;
                //rend.material.color = c;
				//btntxt.text = "Record";
				//Check if there is at least one microphone connected
                canvasGroup = terrainContainer.GetComponent<CanvasGroup>();
                float a = canvasGroup.alpha;
                Debug.Log( "Material alpha: " + a);
                canvasGroup.alpha = 0f;
                Debug.Log( "Material alpha: " + canvasGroup.alpha);
				if(Microphone.devices.Length <= 0)
				{
						//Throw a warning message at the console if there isn't
						Debug.LogWarning("Microphone not connected!");
				}
				else //At least one microphone is present
				{
						//Set 'micConnected' to true
						micConnected = true;

						//Get the default microphone recording capabilities
						Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

						//According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...
						if(minFreq == 0 && maxFreq == 0)
						{
								//...meaning 44100 Hz can be used as the recording sampling rate
								maxFreq = 44100;
						}

						//Get the attached AudioSource component
						goAudioSource = this.GetComponent<AudioSource>();
				}
	}
        IEnumerator FadeIn()
        {
            for (float f = 0.05f; f<=1; f+=0.05f)
            {
                canvasGroup.alpha = f;
                yield return new WaitForSeconds(0.05f);
            }
        }
		public void onRecord() 
		{
				//If there is a microphone
				if(micConnected)
				{
						//If the audio from any microphone isn't being recorded
						if(!Microphone.IsRecording(null))
						{
								//Case the 'Record' button gets pressed
								//if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Record"))
								//{
										//Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
										goAudioSource.clip = Microphone.Start( null, true, 7, maxFreq); //Currently set for a 7 second clip
										btntxt.text = "Stop";
										//stop.SetActive(true);
										//record.SetActive(false);
						}
						else //Recording is in progress
						{
								
								//Case the 'Stop and Play' button gets pressed
								//if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Stop and Play!"))
								//{
										//record.SetActive(true);
										//stop.SetActive(false);
										btntxt.text = "Record";
										float filenameRand = UnityEngine.Random.Range (0.0f, 10.0f);

										string filename = "testing" + filenameRand;

										Microphone.End(null); //Stop the audio recording

										Debug.Log( "Recording Stopped");

										if (!filename.ToLower().EndsWith(".wav")) {
												filename += ".wav";
										}

										var filePath = Path.Combine("testing/", filename);
										filePath = Path.Combine(Application.persistentDataPath, filePath);
										Debug.Log("Created filepath string: " + filePath);

										// Make sure directory exists if user is saving to sub dir.
										Directory.CreateDirectory(Path.GetDirectoryName(filePath));
										SavWav.Save (filePath, goAudioSource.clip); //Save a temporary Wav File
										Debug.Log( "Saving @ " + filePath);
										//Insert your API KEY here.
										string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=AIzaSyDMdQ05ywB8_nAThAhjq2AieCvcz2xu4u0";
										string Response;

										Debug.Log( "Uploading " + filePath);
										Response = HttpUploadFile (apiURL, filePath, "file", "audio/wav; rate=44100");
										//Debug.Log ("Response String: " +Response);

										var jsonresponse = SimpleJSON.JSON.Parse(Response);

										if (jsonresponse != null) {		
												string resultString = jsonresponse ["results"] [0].ToString ();
												var jsonResults = SimpleJSON.JSON.Parse (resultString);

												string transcripts = jsonResults ["alternatives"] [0] ["transcript"].ToString ();

												Debug.Log ("transcript string: " + transcripts );
                                                if(transcripts.Contains("beach")){
                                                        StartCoroutine("FadeIn");
                                                        //terrain.SetActive(true);
                                                        //ocean.SetActive(true);
                                                }   
                                                if(transcripts.Contains("table")){
                                                        table.SetActive(true);
                                                }                                                                                
												//TextBox.text = transcripts;

										}
										goAudioSource.Play(); //Playback the recorded audio

										File.Delete(filePath); //Delete the Temporary Wav file
									
								//}

								//GUI.Label(new Rect(Screen.width/2-100, Screen.height/2+25, 200, 50), "Recording in progress...");
						}
				}
				// else // No microphone
				// {
				// 		//Print a red "Microphone not connected!" message at the center of the screen
				// 		//GUI.contentColor = Color.red;
				// 		//GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Microphone not connected!");
				// }
		}

    public string HttpUploadFile(string url, string file, string paramName, string contentType) {

        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        Debug.Log(string.Format("Uploading {0} to {1}", file, url));

        Byte[] bytes = File.ReadAllBytes(file);
        String file64 = Convert.ToBase64String(bytes,
                                         Base64FormattingOptions.None);

        Debug.Log(file64);

        try
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                


                string json = "{ \"config\": { \"languageCode\" : \"en-US\" }, \"audio\" : { \"content\" : \"" + file64 + "\"}}";

                Debug.Log(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Debug.Log(httpResponse);
            
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Log("Response:" + result);
                return result;

            }
        
        } catch (WebException ex) {
 var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            Debug.Log(resp);
 
}


        return "empty";
		
	}

}
		

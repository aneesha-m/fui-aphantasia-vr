using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]

public class SpeechTextScene : MonoBehaviour
{


    public GameObject terrain;
    public GameObject ocean;
    public GameObject table;
    public GameObject record;
    CanvasGroup canvasGroup;
    //public GameObject stop;
    public Text btntxt = null;
    public Text storyText = null;


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

    //public string apiKey;
    public GameObject frontwall;
    public GameObject leftwall;
    public GameObject rightwall;
    public GameObject backwall;
    public GameObject ceiling;
    public GameObject floor;
    //public GameObject smalltable;
    //public GameObject lamp;
    public GameObject palmTrees;
    public GameObject walkingMan;
    public GameObject walkingWoman;
    public GameObject birds;
    public GameObject bgpeople;
    public AudioSource bgMusic;
    public GameObject pier;
    public GameObject couple;
    private bool canFade;
    private Color alphaColor;
    private float timeToFade = 1.0f;
    //public GameObject pov;
    public GameObject houses;
    //public AudioSource buttonDown;
    //public AudioSource buttonUp;

    public String[] story = new string[] {"I was on a beach on a sunny morning.",
        "I remember some palm trees there.", "and some people walking..","There was a big wooden table.", "And a bunch of flying birds above me.",
        "I also saw a couple sitting", "And a wharf to the right.","There were houses on top of the terrain"};

    // Use this for initialization
    void Start()
    {
        terrain.SetActive(false);
        ocean.SetActive(false);
        table.SetActive(false);
        walkingMan.SetActive(false);
        walkingWoman.SetActive(false);
        birds.SetActive(false);
        bgpeople.SetActive(false);
        palmTrees.SetActive(false);
        storyText.text = story[0];
        pier.SetActive(false);
        couple.SetActive(false);
        canFade = true;
        houses.SetActive(false);
        alphaColor = frontwall.GetComponent<MeshRenderer>().material.color;
        Debug.Log("Material alpha: " + alphaColor);

        if (Microphone.devices.Length <= 0)
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
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate
                maxFreq = 44100;
            }

            //Get the attached AudioSource component
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0; f -= 0.0040f)
        {
            //Color c = rend.material.color;
            Color alphaColor = frontwall.GetComponent<MeshRenderer>().material.color;
            alphaColor.a = f;
            frontwall.GetComponent<MeshRenderer>().material.color = alphaColor;
            backwall.GetComponent<MeshRenderer>().material.color = alphaColor;
            rightwall.GetComponent<MeshRenderer>().material.color = alphaColor;
            leftwall.GetComponent<MeshRenderer>().material.color = alphaColor;
            ceiling.GetComponent<MeshRenderer>().material.color = alphaColor;
            floor.GetComponent<MeshRenderer>().material.color = alphaColor;

            //Debug.Log("Material alpha: " + alphaColor);
            yield return new WaitForSeconds(0.05f);
        }
        frontwall.SetActive(false);
    }
    public async void onRecord()
    {

        //StartCoroutine("SpeechToText");
        SpeechToText();
    }
    //IEnumerator SpeechToText(){
    async void SpeechToText()
    {
        //If there is a microphone
        if (micConnected)
        {
            //If the audio from any microphone isn't being recorded
            if (!Microphone.IsRecording(null))
            {
                //Case the 'Record' button gets pressed
                //if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Record"))
                //{
                //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
                //buttonDown.Play();
                goAudioSource.clip = Microphone.Start(null, true, 15, maxFreq); //Currently set for a 7 second clip
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
                //buttonUp.Play();
                btntxt.text = "Record";
                float filenameRand = UnityEngine.Random.Range(0.0f, 10.0f);

                string filename = "testing" + filenameRand;

                Microphone.End(null); //Stop the audio recording

                Debug.Log("Recording Stopped");

                if (!filename.ToLower().EndsWith(".wav"))
                {
                    filename += ".wav";
                }

                var filePath = Path.Combine("testing/", filename);
                filePath = Path.Combine(Application.persistentDataPath, filePath);
                Debug.Log("Created filepath string: " + filePath);

                // Make sure directory exists if user is saving to sub dir.
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                SavWav.Save(filePath, goAudioSource.clip); //Save a temporary Wav File
                Debug.Log("Saving @ " + filePath);
                //Insert your API KEY here.
                string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=AIzaSyBQg87BmqhL1A1ogn78I34CnVT2qAx-0yc";
                string Response;

                Debug.Log("Uploading " + filePath);
                Response = HttpUploadFile(apiURL, filePath, "file", "audio/wav; rate=44100");
                //Debug.Log ("Response String: " +Response);

                var jsonresponse = SimpleJSON.JSON.Parse(Response);

                if (jsonresponse != null)
                {
                    string resultString = jsonresponse["results"][0].ToString();
                    var jsonResults = SimpleJSON.JSON.Parse(resultString);

                    string transcripts = jsonResults["alternatives"][0]["transcript"].ToString();

                    Debug.Log("transcript string: " + transcripts);
                    if (transcripts.ToLower().Contains("beach"))
                    {

                        terrain.SetActive(true);
                        ocean.SetActive(true);
                        StartCoroutine("FadeOut");
                        // frontwall.SetActive(false);
                        // backwall.SetActive(false);
                        // rightwall.SetActive(false);
                        // leftwall.SetActive(false);
                        // ceiling.SetActive(false);
                        // floor.SetActive(false);
                        bgMusic.Play();
                        storyText.text = story[1];
                    }
                    if (transcripts.ToLower().Contains("trees") || transcripts.ToLower().Contains("palm"))
                    {
                        palmTrees.SetActive(true);
                        storyText.text = story[2];
                    }
                    if (transcripts.ToLower().Contains("walking"))
                    {

                        walkingMan.SetActive(true);
                        walkingWoman.SetActive(true);
                        storyText.text = story[3];
                    }
                    if (transcripts.ToLower().Contains("birds") || transcripts.ToLower().Contains("flying"))
                    {

                        birds.SetActive(true);
                        storyText.text = story[5];
                    }
                    if (transcripts.ToLower().Contains("people"))
                    {

                        bgpeople.SetActive(true);
                    }
                    if (transcripts.Contains("table"))
                    {
                        table.SetActive(true);
                        storyText.text = story[4];
                    }
                    if (transcripts.Contains("wharf") || transcripts.Contains("pier") || transcripts.Contains("right"))
                    {
                        pier.SetActive(true);
                        storyText.text = story[7];
                    }
                    if (transcripts.Contains("couple"))
                    {
                        couple.SetActive(true);
                        storyText.text = story[6];
                    }
                    if (transcripts.Contains("house") || transcripts.Contains("houses"))
                    {
                        houses.SetActive(true);
                    }
                    //TextBox.text = transcripts;

                }
                //goAudioSource.Play(); //Playback the recorded audio

                File.Delete(filePath); //Delete the Temporary Wav file

                //}

                //GUI.Label(new Rect(Screen.width/2-100, Screen.height/2+25, 200, 50), "Recording in progress...");
            }
        }
        else // No microphone
        {
            btntxt.text = "No mic";
        }
        //yield return new WaitForSeconds(0.01f);
        //await TimeSpan.FromSeconds(1);
        // return true;

    }

    public string HttpUploadFile(string url, string file, string paramName, string contentType)
    {

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

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            Debug.Log(resp);

        }


        return "empty";

    }

}


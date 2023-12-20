﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LivingParticlesAudioSource : MonoBehaviour
{
    public AudioClip audioClip;

    [Range(0.1f, 2f)]
    public float bufferInitialDecreaseSpeed = 1f;
    [Range(0f, 10f)]
    public float bufferDecreaseSpeedMultiply = 5f;
    public float freqBandsPower = 10f;
    public float audioProfileInitialValue = 5f;
    public bool audioProfileDecreasing = true;
    public float audioProfileDecreasingSpeed = 0.1f;

    public enum _numberOfBands { Bands8, Bands16 }
    public _numberOfBands numberOfBands = _numberOfBands.Bands8;

    private float[] initialSamplesL = new float[512];
    private float[] initialSamplesR = new float[512];

    private float[] freqBands8 = new float[8];
    private float[] freqBands8Buffer = new float[8];
    private float[] freqBands8BufferDecrease = new float[8];
    private float[] freqBands8Highest = new float[8];

    private float[] freqBands16 = new float[16];
    private float[] freqBands16Buffer = new float[16];
    private float[] freqBands16BufferDecrease = new float[16];
    private float[] freqBands16Highest = new float[16];

    private AudioSource audioSource;


    [HideInInspector] public float[] finalBands8 = new float[8];
    [HideInInspector] public float[] finalBands8Buffer = new float[8];

    [HideInInspector] public float[] finalBands16 = new float[16];
    [HideInInspector] public float[] finalBands16Buffer = new float[16];

    [HideInInspector] public float amplitude8, amplitudeBuffer8;
    [HideInInspector] public float amplitude16, amplitudeBuffer16;
    private float amplitudeHighest;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfileInitialValue);

        // CreateWindow(512, false);

        // readWav($"C:\\Users\\dwelx\\Documents\\GitHub\\BadAssEmotes\\ExamplePlugin\\Ifu.ogg", out left, out right, out samplerate, out channels);
        //audioSource.GetSpectrumData(initialSamplesL, 0, FFTWindow.Blackman);
        //audioSource.GetSpectrumData(initialSamplesR, 1, FFTWindow.Blackman);
    }
    public void StartAudio()
    {
        Reset();
        //TODO audio
        //var result = AkAudioInputManager.PostAudioInputEvent("IFUTestInput", gameObject, WavBufferToWwise, BeforePlayingAudio);
    }

    public void Reset()
    {
        _length = 0;
    }

    private static bool speaking = false;
    private uint _length = 0;

    //TODO audio
    //private void BeforePlayingAudio(uint playingID, AkAudioFormat format)
    //{
    //    uint samplerate, channels;

    //    left = right = new float[0];

    //    readWav($"C:\\Users\\dwelx\\Documents\\GitHub\\BadAssEmotes\\ExamplePlugin\\Ifu.ogg", out left, out right, out samplerate, out channels);
    //    format.channelConfig.uNumChannels = channels;
    //    format.uSampleRate = samplerate;

    //    yote = true;
    //}
    bool yote = false;

    static bool readWav(string filename, out float[] L, out float[] R, out uint samplerate, out uint channels)
    {
        speaking = true;
        L = R = null;

        samplerate = 0;
        channels = 0;

        try
        {
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fs);

                // chunk 0
                int chunkID = reader.ReadInt32();
                int fileSize = reader.ReadInt32();
                int riffType = reader.ReadInt32();


                // chunk 1
                int fmtID = reader.ReadInt32();
                int fmtSize = reader.ReadInt32(); // bytes for this chunk (expect 16 or 18)

                // 16 bytes coming...
                int fmtCode = reader.ReadInt16();
                int Channels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                int byteRate = reader.ReadInt32();
                int fmtBlockAlign = reader.ReadInt16();
                int bitDepth = reader.ReadInt16();

                samplerate = (uint)sampleRate;
                channels = (uint)Channels;

                if (fmtSize == 18)
                {
                    // Read any extra values
                    int fmtExtraSize = reader.ReadInt16();
                    reader.ReadBytes(fmtExtraSize);
                }

                // chunk 2
                int dataID = reader.ReadInt32();
                int bytes = reader.ReadInt32();

                // DATA!
                byte[] byteArray = reader.ReadBytes(bytes);

                int bytesForSamp = bitDepth / 8;
                int nValues = bytes / bytesForSamp;


                float[] asFloat = null;
                switch (bitDepth)
                {
                    case 64:
                        double[]
                            asDouble = new double[nValues];
                        Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
                        asFloat = Array.ConvertAll(asDouble, e => (float)e);
                        break;
                    case 32:
                        asFloat = new float[nValues];
                        Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
                        break;
                    case 16:
                        Int16[]
                            asInt16 = new Int16[nValues];
                        Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
                        asFloat = Array.ConvertAll(asInt16, e => e / (float)(Int16.MaxValue + 1));
                        break;
                    default:
                        return false;
                }

                switch (channels)
                {
                    case 1:
                        L = asFloat;
                        R = null;
                        return true;
                    case 2:
                        // de-interleave
                        int nSamps = nValues / 2;
                        L = new float[nSamps];
                        R = new float[nSamps];
                        for (int s = 0, v = 0; s < nSamps; s++)
                        {
                            L[s] = asFloat[v++];
                            R[s] = asFloat[v++];
                        }
                        return true;
                    default:
                        return false;
                }
            }
        }
        catch
        {
            Debug.Log("...Failed to load: " + filename);
            return false;
        }

        return false;
    }
    float[] left, right;
    private bool WavBufferToWwise(uint playingID, uint channelIndex, float[] samples)
    {
        if (left.Length <= 0)
        {
            DebugClass.Log("There was an error playing the audio file, The audio buffer is empty!");
        }


        // probably redundant now tbh.
        if (_length >= (uint)left.Length)
        {
            _length = (uint)left.Length;
        }

        // DebugClass.Log($"Samples: {samples.Length}, Left: {left.Length}, Current: {length}");

        initialSamplesL = new float[samples.Length];
        initialSamplesR = new float[samples.Length];

        try
        {
            uint i = 0;

            for (i = 0; i < samples.Length; ++i)
            {
                if (i + _length >= left.Length)
                {
                    speaking = false;
                    //TODO audio
                    //AkSoundEngine.ExecuteActionOnEvent(3183910552, AkActionOnEventType.AkActionOnEventType_Stop);
                    _length = 0;
                    break;
                }

                initialSamplesL[i] = left[i + _length];
                initialSamplesR[i] = right[i + _length];

                samples[i] = 0;
            }

            _length += i;
        }
        catch (Exception)
        {
            Debug.Log($"--------end of audio???");
            throw;
        }
        if (_length >= (uint)left.Length)
        {
            _length = (uint)left.Length;
            speaking = false;
        }

        //DebugClass.Log($"id:{playingID}, samples: {samples}, channlIndex: {channelIndex}");

        DoSpectrumDataStuff();

        return speaking;
    }

    void ApplyWindow()
    {
        var length = initialSamplesL.Length;
        CreateWindow(length);

        for (int i = 0; i < length; i++)
        {
            initialSamplesL[i] = (float)(initialSamplesL[i] * _window[i]);
            initialSamplesR[i] = (float)(initialSamplesR[i] * _window[i]);
        }
    }

    void DoSpectrumDataStuff()
    {
        //audioSource.GetSpectrumData(initialSamplesL, 0, FFTWindow.Blackman);
        //audioSource.GetSpectrumData(initialSamplesR, 1, FFTWindow.Blackman);

        // GetSpectrumData(512);

        ApplyWindow();

        switch (numberOfBands)
        {
            case _numberOfBands.Bands8:
                CreateFreqBands8();
                CreateBandBuffer8();
                CreateFinalBands8();
                CreateAmplitude8();
                break;
            case _numberOfBands.Bands16:
                CreateFreqBands16();
                CreateBandBuffer16();
                CreateFinalBands16();
                CreateAmplitude16();
                break;
            default:
                break;
        }
    }

    private double[] _window;

    void CreateWindow(int size, bool normalize = false)
    {
        const double a = 0.42659071;
        const double b = 0.49656062;
        const double c = 0.07684867;

        _window = new double[size];
        
        for (int i = 0; i < size; i++)
        {
            double frac = (double)i / (size - 1);
            _window[i] = 0.42f - (0.5f * Mathf.Cos(i / size) ) + (0.08 * Mathf.Cos(2.0f * i / size));
        }

        if (normalize)
        {
            double sum = 0;

            for (int i = 0; i < _window.Length; i++)
            {
                sum += _window[i];
            }

            for (int i = 0; i < _window.Length; i++)
            {
                _window[i] /= sum;
            }
        }
    }

    //void GetSpectrumData(int sampleSize)
    //{
    //    float[] leftSamples = new float[sampleSize];
    //    float[] rightSamples = new float[sampleSize];

    //    GetSamples(leftSamples, rightSamples);

    //    for (int i = 0; i < sampleSize; i++)
    //    {
    //        initialSamplesL[i] = (float)(leftSamples[i] * _window[i]);
    //        initialSamplesR[i] = (float)(rightSamples[i] * _window[i]);
    //    }
    //}
    

    //void GetSamples(float[] leftSamples, float[] rightSamples)
    //{
    //    for (int i = 0; i < leftSamples.Length; i++)
    //    {
    //        leftSamples[i] = left[_offset + i];
    //        rightSamples[i] = right[_offset + i];
    //    }

    //    _offset += leftSamples.Length;
    //}

    void AudioProfile(float audioProfileValue)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBands8Highest[i] = audioProfileValue;
        }
        for (int i = 0; i < 16; i++)
        {
            freqBands16Highest[i] = audioProfileValue;
        }
    }

    // Creating average amplitude 8
    void CreateAmplitude8()
    {
        float currentAmplitude = 0f;
        float currentAmplitudeBuffer = 0f;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += finalBands8[i];
            currentAmplitudeBuffer += finalBands8Buffer[i];
        }
        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        amplitude8 = currentAmplitude / amplitudeHighest;
        amplitudeBuffer8 = currentAmplitudeBuffer / amplitudeHighest;
    }

    // Creating average amplitude 16
    void CreateAmplitude16()
    {
        float currentAmplitude = 0f;
        float currentAmplitudeBuffer = 0f;
        for (int i = 0; i < 16; i++)
        {
            currentAmplitude += finalBands16[i];
            currentAmplitudeBuffer += finalBands16Buffer[i];
        }
        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        amplitude16 = currentAmplitude / amplitudeHighest;
        amplitudeBuffer16 = currentAmplitudeBuffer / amplitudeHighest;
    }

    // Creating Final Bands 8 to use in the Material
    void CreateFinalBands8()
    {
        for (int i = 0; i < 8; i++)
        {
            if (audioProfileDecreasing == true)
            {
                freqBands8Highest[i] -= audioProfileDecreasingSpeed * Time.deltaTime;
            }

            if (freqBands8[i] > freqBands8Highest[i])
            {
                freqBands8Highest[i] = freqBands8[i];
            }
            finalBands8[i] = freqBands8[i] / freqBands8Highest[i];
            finalBands8Buffer[i] = freqBands8Buffer[i] / freqBands8Highest[i];
        }
    }

    // Creating Final Bands 16 to use in the Material
    void CreateFinalBands16()
    {
        for (int i = 0; i < 16; i++)
        {
            if (audioProfileDecreasing == true)
            {
                freqBands16Highest[i] -= audioProfileDecreasingSpeed * Time.deltaTime;
            }

            if (freqBands16[i] > freqBands16Highest[i])
            {
                freqBands16Highest[i] = freqBands16[i];
            }
            finalBands16[i] = freqBands16[i] / freqBands16Highest[i];
            finalBands16Buffer[i] = freqBands16Buffer[i] / freqBands16Highest[i];
        }
    }

    // Creating 8 Freq Bands Buffer
    void CreateBandBuffer8()
    {
        for (int y = 0; y < 8; y++)
        {
            if (freqBands8[y] > freqBands8Buffer[y])
            {
                freqBands8Buffer[y] = freqBands8[y];
                freqBands8BufferDecrease[y] = bufferInitialDecreaseSpeed * freqBands8Highest[y] * Time.deltaTime; // You can delete the second part of multiplication
            }
            if (freqBands8[y] < freqBands8Buffer[y])
            {
                freqBands8Buffer[y] -= freqBands8BufferDecrease[y];
                freqBands8BufferDecrease[y] *= 1f + (bufferDecreaseSpeedMultiply * Time.deltaTime);
            }
        }
    }

    // Creating 16 Freq Bands Buffer
    void CreateBandBuffer16()
    {
        for (int y = 0; y < 16; y++)
        {
            if (freqBands16[y] > freqBands16Buffer[y])
            {
                freqBands16Buffer[y] = freqBands16[y];
                freqBands16BufferDecrease[y] = bufferInitialDecreaseSpeed * freqBands16Highest[y] * Time.deltaTime; // You can delete the second part of multiplication
            }

            if (freqBands16[y] < freqBands16Buffer[y])
            {
                freqBands16Buffer[y] -= freqBands16BufferDecrease[y];
                freqBands16BufferDecrease[y] *= 1f + (bufferDecreaseSpeedMultiply * Time.deltaTime);
            }
        }
    }

    // Creating 8 Freq Bands
    void CreateFreqBands8()
    {
        int num = 1;
        //TODO IFU audio
        //AkSoundEngine.GetRTPCValue("Band1", null, 0U, out freqBands8[0], ref num);
        //freqBands8[0] = (freqBands8[0] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band1", null, 0U, out freqBands8[1], ref num);
        //freqBands8[1] = (freqBands8[1] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band2", null, 0U, out freqBands8[2], ref num);
        //freqBands8[2] = (freqBands8[2] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band2", null, 0U, out freqBands8[3], ref num);
        //freqBands8[3] = (freqBands8[3] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band3", null, 0U, out freqBands8[4], ref num);
        //freqBands8[4] = (freqBands8[4] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band3", null, 0U, out freqBands8[5], ref num);
        //freqBands8[5] = (freqBands8[5] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band4", null, 0U, out freqBands8[6], ref num);
        //freqBands8[6] = (freqBands8[6] + 48) / 48;
        //AkSoundEngine.GetRTPCValue("Band4", null, 0U, out freqBands8[7], ref num);
        //freqBands8[7] = (freqBands8[7] + 48) / 48;

        //int count = 0;

        //for (int i = 0; i < 8; i++)
        //{
        //    float average = 0f;
        //    int sampleCount = (int)Mathf.Pow(2, i) * 2;

        //    if (i == 7)
        //    {
        //        sampleCount += 2;
        //    }
        //    for (int j = 0; j < sampleCount; j++)
        //    {
        //        //DebugClass.Log($"----------  {i}  L: {initialSamplesL[count]}   R: {initialSamplesR[count]}");
        //        average += (initialSamplesL[count] + initialSamplesR[count]) * (count + 1);
        //        count++;
        //    }
        //    //DebugClass.Log($"---------- AVERAGE CALCULATION:  {i}   {average} /= {count}");
        //    average /= count;

        //    freqBands8[i] = average * freqBandsPower;
        //    //DebugClass.Log($"----------         {i}   {freqBands8[i]}");
        //}
    }

    // Creating 16 Freq Bands
    void CreateFreqBands16()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        /*
         * 0 - 1 =   1 samples > 2
         * 2 - 3 =   2 samples > 4
         * 4 - 5 =   4 samples > 8
         * 6 - 7 =  8 samples > 16
         * 8 - 9 = 16 samples > 32
         * 10 - 11 = 32 samples > 64
         * 12 - 13 = 64 samples > 128
         * 14 - 15 = 128 samples > 256
         */

        for (int i = 0; i < 16; i++)
        {
            float average = 0f;

            if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 7)
                {
                    sampleCount += 1;
                }
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += (initialSamplesL[count] + initialSamplesR[count]) * (count + 1);
                count++;
            }

            average /= count;

            freqBands16[i] = average * freqBandsPower * 2;
        }
    }
}

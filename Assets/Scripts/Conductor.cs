using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public int countCreateBefore = 5;
    private readonly int quality = 10;
    private int sampleCount = 0;
    private int freq;
    private readonly float debugLineWidth = 5;
    private float maxHieght = 0;
    private float minHieght = 1;

    private float[] waveFormArray;
    private float[] samples;

    private AudioSource myAudio;
    public Transform cube;
    public Material[] materials;
    public GameObject[] objectsToOn;
    public AudioClip CongratuateClip;
    private Transform cameraPoint;

    int lastIndex = -1;
    int lastCreatedIndex = -1;
    private bool isFifshed;

    private void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
        cameraPoint = transform.GetChild(0);

        //Базовые расчеты
        freq = myAudio.clip.frequency;
        sampleCount = freq / quality;

        //Получение аудиоданных
        samples = new float[myAudio.clip.samples * myAudio.clip.channels];
        myAudio.clip.GetData(samples, 0);

        //Создание массива с данными для отрисовки аудиоформы
        int lastPickeIndex = 0;
        bool isUpping = false;
        waveFormArray = new float[(samples.Length / sampleCount)];
        for (int i = 0; i < waveFormArray.Length; i++)
        {
            waveFormArray[i] = 0;
            for (int j = 0; j < sampleCount; j++)
            {
                waveFormArray[i] += Mathf.Abs(samples[(i * sampleCount) + j]);
            }
            waveFormArray[i] /= sampleCount;
            if (i > 0)
            {
                if (waveFormArray[i - 1] < waveFormArray[i])
                    isUpping = true;
                if (waveFormArray[i - 1] > waveFormArray[i])
                {
                    if (isUpping)
                    {
                        for (int j = lastPickeIndex; j < i - 1; j++)
                            waveFormArray[j] = 0;
                        lastPickeIndex = i;
                    }
                    isUpping = false;
                }
            }
        }

        

        // заранее создаем кубы
        for (int i = 0; i < countCreateBefore; i++)
        {
            if (waveFormArray[i] > 0)
            {
                int position = (int) (waveFormArray[i] / 0.2f) * 2;
                Transform newCube = Instantiate(cube, new Vector3(), Quaternion.identity);
                newCube.position = new Vector3(position, 0, i);
                newCube.GetChild(0).GetComponent<MeshRenderer>().material = materials[position/2];
            }
        }

        
        for (int i = 0; i < waveFormArray.Length - 1; i++)
        {
            ////Создание вектора для верхней половины аудиоформы
            //Vector3 upLine = new Vector3(i * 0.01f, waveFormArray[i] * 10, 0);
            ////Создание вектора для нижней половины аудиоформы
            //Vector3 downLine = new Vector3(i * 0.01f, -waveFormArray[i] * 10, 0);
            ////Отрисовка Debug информации
            //Debug.DrawLine(upLine, downLine, Color.green);
            if (waveFormArray[i] > maxHieght) maxHieght = waveFormArray[i];
            if (waveFormArray[i] < minHieght) minHieght = waveFormArray[i];
        }

        lastCreatedIndex = countCreateBefore;
        myAudio.Play();

        
    }

    private void Update()
    {
        

        //Создание "бегунка" на аудиоформе. Положение привязано к текущему временному сэмплу
        int currentPosition = myAudio.timeSamples / sampleCount * myAudio.clip.channels;
        if (currentPosition + countCreateBefore < waveFormArray.Length)
            if (currentPosition + countCreateBefore > lastCreatedIndex)
            {
                for (int i=lastCreatedIndex + 1; i < currentPosition + 1 + countCreateBefore; i++)
                {
                    if (waveFormArray[i] > 0)
                    {
                        int position = (int)(((waveFormArray[i] - minHieght)/(maxHieght-minHieght)) * (materials.Length-1));
                         Transform newCube = Instantiate(cube, new Vector3(), Quaternion.identity);
                        newCube.position = new Vector3(position*2, 0, i);
                        newCube.GetChild(0).GetComponent<MeshRenderer>().material = materials[position];
                    }
                }
                lastCreatedIndex = currentPosition + countCreateBefore;
            }
        float curPos = (float)myAudio.timeSamples / sampleCount * myAudio.clip.channels;
        cameraPoint.transform.position = new Vector3(0, 0, curPos);

        //Vector3 drawVector = new Vector3(currentPosition * 0.01f, 0, 0);
        //Debug.DrawLine(drawVector - Vector3.up * debugLineWidth, drawVector + Vector3.up * debugLineWidth, Color.white);
        if (!myAudio.isPlaying && !isFifshed)
        {
            foreach (GameObject a in objectsToOn) a.SetActive(true);
            myAudio.clip = CongratuateClip;
            myAudio.Play();
            isFifshed = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Globalization;

public class ParameterManager : MonoBehaviour
{

    [Header("Field of view size")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    [Tooltip("This is the size of the radius.")]
    private float fieldOfViewSize = 1.0f;
    [SerializeField]
    [Range(0, 360)]
    [Tooltip("This is the size of blind spot of the agent (in degrees)")]
    protected float blindSpotSize = 30;

    [Header("Intensity parameters")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float cohesionIntensity = 1.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float alignmentIntensity = 1.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float separationIntensity = 1.0f;
    [SerializeField]
    [Range(0.0f, 20.0f)]
    private float avoidingObstaclesIntensity = 1.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float moveForwardIntensity = 1.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float randomMovementIntensity = 20.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float frictionIntensity = 0.1f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float maxSpeed = 1.0f;


    private float temps_ecoule; //calcul le temps ecoule depuis apres le start  
    private int indice_timecode; //permet de savoir a quelle ligne du csv des timecode on se situe
    private int indice_donnee; //permet de savoir a quelle indice des donnee freq amp et db on se situe dans le csv/list
    private float prochain_beat; // contient le temps en seconde de la prochiane pulsation venant de Timecode_beat

    private List<string> list_Temps = new List<string>();  //liste des temps des donnee des liste suivante
    private List<string> list_Freq = new List<string>(); // liste des valeur de frequence
    private List<string> list_Amp = new List<string>(); //liste des valeurs d'amplitude
    private List<string> list_dB = new List<string>(); // liste des valeursd d'intensite

    private List<string> list_timecode_beats = new List<string>(); //liste en seconde des pulsation


    void Start()
    {
        StreamReader reader_donnee = new StreamReader(File.OpenRead(@"C:\Users\Alexandre\Desktop\python_tal_final\Temps_frequence_amplitude_dB.csv"));

        while (!reader_donnee.EndOfStream)
        {
            var line = reader_donnee.ReadLine();
            var values = line.Split(",");

            list_Temps.Add(values[0]);
            list_Freq.Add(values[1]);
            list_Amp.Add(values[2]);
            list_dB.Add(values[3]);
        }

        StreamReader reader_timecode = new StreamReader(File.OpenRead(@"C:\Users\Alexandre\Desktop\python_tal_final\Timecode_Beat.csv"));
        while (!reader_timecode.EndOfStream)
        {
            var line = reader_timecode.ReadLine();
            var values = line.Split(",");

            list_timecode_beats.Add(values[1]);
        }

        temps_ecoule = 0.0f;
        indice_timecode = 1;
        indice_donnee = 1;
        prochain_beat = float.Parse(list_timecode_beats[indice_timecode], CultureInfo.InvariantCulture.NumberFormat);

    }

    void Update()
    {
        temps_ecoule += Time.deltaTime;
        if (temps_ecoule > prochain_beat) //si le temps ecoule correspond ou et superieur au temps de la prochaine pulsation alors on effectue un changement des parametres
        {
            //Debug.Log(prochain_beat);
            while (prochain_beat > float.Parse(list_Temps[indice_donnee], CultureInfo.InvariantCulture.NumberFormat))
            {
                indice_donnee += 1; // tant que le temps des donnes est inferieur au temps du prochain beat on tourne pour trouver la bonne ligne qui correspond dans les donnees
            }

            SetSeparationIntensity(float.Parse(list_Amp[indice_donnee], CultureInfo.InvariantCulture.NumberFormat));
            SetCohesionIntensity(float.Parse(list_Freq[indice_donnee], CultureInfo.InvariantCulture.NumberFormat));


            indice_timecode += 1;
            prochain_beat = float.Parse(list_timecode_beats[indice_timecode], CultureInfo.InvariantCulture.NumberFormat);
        }
    }



    #region Methods - Getter

    public float GetFieldOfViewSize()
    {
        return fieldOfViewSize;
    }

    public float GetBlindSpotSize()
    {
        return blindSpotSize;
    }

    public float GetSeparationIntensity()
    {
        return separationIntensity;
    }

    public float GetAlignmentIntensity()
    {
        return alignmentIntensity;
    }

    public float GetCohesionIntensity()
    {
        return cohesionIntensity;
    }

    public float GetAvoidingObstaclesIntensity()
    {
        return avoidingObstaclesIntensity;
    }

    public float GetMoveForwardIntensity()
    {
        return moveForwardIntensity;
    }

    public float GetRandomMovementIntensity()
    {
        return randomMovementIntensity;
    }

    public float GetFrictionIntensity()
    {
        return frictionIntensity;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    #endregion


    #region Methods : Setter
    public void SetFieldOfViewSize(float val)
    {
        this.fieldOfViewSize = val;
    }

    public void SetBlindSpotSize(float val)
    {
        this.blindSpotSize = val;
    }

    public void SetSeparationIntensity(float val)
    {
        //this.separationIntensity = val;
        if (val <= 0.15f)
        {
            this.separationIntensity = 0.0f;
        }
        else if (val >= 1.0f)
        {
            this.separationIntensity = 5.0f;
        }
        else
        {
            this.separationIntensity = 2.5f;
        }
    }

    public void SetAlignmentIntensity(int val)
    {
        //this.alignmentIntensity = val;
        if (val == 3)
        {
            this.alignmentIntensity = 0.0f;
        }
        else if (val == 1)
        {
            this.alignmentIntensity = 5.0f;
        }
        else
        {
            this.alignmentIntensity = 2.5f;
        }
    }

    public void SetCohesionIntensity(float val)
    {
        //this.cohesionIntensity = val;
        if (val <= 1.5f)
        {
            this.cohesionIntensity = 0.0f;
        }
        else if (val >= 7.0f)
        {
            this.cohesionIntensity = 5.0f;
        }
        else
        {
            this.cohesionIntensity = 2.5f;
        }
    }


    public void SetMoveForwardIntensity(float val)
    {
        this.moveForwardIntensity = val;
    }

    public void SetRandomMovementIntensity(float val)
    {
        this.randomMovementIntensity = val;
    }

    public void SetFrictionIntensity(float val)
    {
        this.frictionIntensity = val;
    }


    public void SetMaxSpeed()
    {
        if (this.maxSpeed >= 1.0f)
        {
            this.maxSpeed = 0.0f;
        }
        else
        {
            this.maxSpeed += 0.1f;
        }
    }

    #endregion

}


//float valeur = float.Parse(list_timecode_beats[i], CultureInfo.InvariantCulture.NumberFormat);

// cohesion
// alignement
// separation
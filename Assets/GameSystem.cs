using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Data
{
    public static int DataLevel, DataScore, DataWaktu, DataDarah;
}

public class GameSystem : MonoBehaviour
{

    public static GameSystem instance;
    int MaxLevel = 5;

    
    [Header("Data Permainan")]
    public bool GameAktif;
    public bool GameSelesai;
    public bool SistemAcak;
    public int Target,DataSaatIni;
    


    [Header("Komponen UI")]
    public TextMeshProUGUI Teks_Level;
    public TextMeshProUGUI Teks_Waktu, Teks_Score;
    public RectTransform Ui_Darah;

    [Header("Obj UI")]
    public GameObject Gui_Pause;
    public GameObject Gui_Transisi;


    [System.Serializable]
    public class DataGame
    {
        public string Nama;
        public Sprite Gambar;
    }

    [Header("Settingan Standar")]
    public DataGame[] DataPermainan;

    [Space]
    [Space]
    [Space]
    public Obj_TempatDrop[] Drop_Tempat;
    public Obj_Drag[] Drag_Obj;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameAktif = false;
        GameSelesai = false;
        ResetData();
        Target = Drop_Tempat.Length;
        if(SistemAcak)
            AcakSoal();
        
        DataSaatIni = 0;
        GameAktif = true;
    }
    

    void ResetData()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name =="Game0")
        {
            Data.DataWaktu = 60 * 3;
            Data.DataScore = 0;
            Data.DataDarah = 5;
            Data.DataLevel = 0;
        }
    }

    float s;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AcakSoal();

        if (GameAktif && !GameSelesai)
        {
            if(Data.DataWaktu > 0)
            {
                s += Time.deltaTime;
                if(s >= 1)
                {
                    Data.DataWaktu--;
                    s = 0; 
                }
            }
            if (Data.DataWaktu <= 0)
            {
                GameAktif = false;
                GameSelesai = true;

                //game kalah
                KumpulanSuara.instance.Panggil_sfx(4);
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
            }

            if (Data.DataDarah <= 0)
            {
                GameSelesai = true;
                GameAktif = false;

                // Fungsi kalah
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
                KumpulanSuara.instance.Panggil_sfx(4);
                
            }

            if(DataSaatIni >= Target)
            {
                GameSelesai = true;
                GameAktif = false;

                //game menang
                if(Data.DataLevel < (MaxLevel -1))
                {
                    Data.DataLevel++;
                // Pindah Level

                UnityEngine.SceneManagement.SceneManager.LoadScene("Game" + Data.DataLevel);
                // Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("Game" + Data.DataLevel);

                KumpulanSuara.instance.Panggil_sfx(3);
                }
                else
                {
                    // Game Selesai pindah ke menu selesai
                    Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
                    KumpulanSuara.instance.Panggil_sfx(5);
                    KumpulanSuara.instance.Panggil_sfx(6);
                }
            }
        }

        SetInfoUI();
    }
    
    [HideInInspector]public List<int> _AcakSoal = new List<int>();
    [HideInInspector]public List<int> _AcakPos = new List<int>();
    int rand;
    int rand2;
    public void AcakSoal()
    {
        _AcakSoal.Clear();
        _AcakPos.Clear();

        _AcakSoal = new List<int>(new int[Drag_Obj.Length]);

        for (int i = 0; i < _AcakSoal.Count; i++)
        {
            rand = Random.Range(1, DataPermainan.Length);
            while (_AcakSoal.Contains(rand))
                rand = Random.Range(1, DataPermainan.Length);

            _AcakSoal[i] = rand;

            Drag_Obj[i].ID = rand -1;
            Drag_Obj[i].Teks.text = DataPermainan[rand - 1].Nama;

        }

        _AcakPos = new List<int>(new int[Drop_Tempat.Length]);

        for (int i = 0; i < _AcakPos.Count; i++)
        {
            rand2 = Random.Range(1, _AcakSoal.Count + 1);
            while (_AcakPos.Contains(rand2))
                rand2 = Random.Range(1, _AcakSoal.Count + 1);

            _AcakPos[i] = rand2;

            Drop_Tempat[i].Drop.ID = _AcakSoal[rand2 - 1] - 1;
            Drop_Tempat[i].Gambar.sprite = DataPermainan[Drop_Tempat[i].Drop.ID].Gambar;


    }
    }

    public void SetInfoUI()
    {
        Teks_Level.text = (Data.DataLevel + 1).ToString() ;

        int Menit = Mathf.FloorToInt(Data.DataWaktu / 60);
        int Detik = Mathf.FloorToInt(Data.DataWaktu % 60);
        Teks_Waktu.text = Menit.ToString("00") + ":" + Detik.ToString("00");

        Teks_Score.text = Data.DataScore.ToString();

        Ui_Darah.sizeDelta =  new Vector2(50f * Data.DataDarah , 50f);
    }

    public void Btn_Pause(bool pause)
    {
        if (pause)
        {
            GameAktif =  false;
            Gui_Pause.SetActive(true);
        }
        else
        {
            GameAktif =  true;
            Gui_Pause.SetActive(false);
        }
    }



}


﻿using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using DEEP.HUD;
using DEEP.Weapons;
using DEEP.Entities.Player;

namespace DEEP.UI { 

    public class OptionsButtons : MonoBehaviour
    {

        [SerializeField] Slider mouseSensitivity = null;//mouse sensitivity slider
        [SerializeField] Toggle rightSideWeapon = null;// Weapon on the right side of the screen toggle

        [SerializeField] Toggle fullscreen = null;//fullscreen toggle
        [SerializeField] Dropdown quality = null;//dropdown quality
        [SerializeField] Dropdown resolutionDropdown = null;//Dropdown resolution
        Resolution[] resolution;//vetor de resol.
        [SerializeField] Slider volume = null;//volume sensitivity slider

        [SerializeField] Toggle speedrun = null;// Speedrun HUD toggle

        [SerializeField] Toggle statistics = null;// Statistics hUD toggle

        // Initializes the options, should be called by the main menu.
        public void Awake()
        {
            
            //pega o valor inicial da sensibilidade do mouse
            if(!PlayerPrefs.HasKey("Mouse sensitivity"))
                PlayerPrefs.SetFloat("Mouse sensitivity", 7.0f);
            mouseSensitivity.value = PlayerPrefs.GetFloat("Mouse sensitivity");

            // Gets initial right side weapon value.
            if(!PlayerPrefs.HasKey("RightSideWeapon"))
                PlayerPrefs.SetInt("RightSideWeapon", 0);
            rightSideWeapon.isOn = (PlayerPrefs.GetInt("RightSideWeapon") != 0);

            resolution = Screen.resolutions;//get the possible resolutions 
            resolutionDropdown.ClearOptions();//clear the dropdown

            List<string> options = new List<string>();//cria uma lista de strings para o dropdown

            int currentindex = 0;
            for (int i = 0; i < resolution.Length; i++)
            {
                string option = resolution[i].width + " X " + resolution[i].height + " (" + resolution[i].refreshRate + "Hz)";// string no formato [widht X height (refreshRate)]
                options.Add(option);//adiciona a nova opcao na string

                if (resolution[i].height == Screen.height && resolution[i].width == Screen.width)
                    currentindex = i;//indica qual e o index da resolucao atual
                
            }

            resolutionDropdown.AddOptions(options);//adiciona as novas opcoes
            resolutionDropdown.value = currentindex;//adiciona a selecao atual
            resolutionDropdown.RefreshShownValue();//atualiza

            quality.value = QualitySettings.GetQualityLevel();
            fullscreen.isOn = Screen.fullScreen;   

            //pega o valor inicial do volume
            if(!PlayerPrefs.HasKey("Volume"))
                PlayerPrefs.SetFloat("Volume", 1.0f);
            volume.value = PlayerPrefs.GetFloat("Volume");

            // Gets initial speedrun HUD value.
            if(!PlayerPrefs.HasKey("SpeedrunHUD"))
                PlayerPrefs.SetInt("SpeedrunHUD", 0);
            speedrun.isOn = (PlayerPrefs.GetInt("SpeedrunHUD") != 0);

            // Gets initial statistics HUD value.
            if(!PlayerPrefs.HasKey("StatisticsHUD"))
                PlayerPrefs.SetInt("StatisticsHUD", 0);
            statistics.isOn = (PlayerPrefs.GetInt("StatisticsHUD") != 0);

        }

        //muda a sensibilidade do mouse
        public void SetMouseSensitivity()
        {

            PlayerPrefs.SetFloat("Mouse sensitivity", mouseSensitivity.value);

            // Tries to update mouse sentitivity on player's movementation script.
            PlayerMovementation movementation = FindObjectOfType<PlayerMovementation>();
            if(movementation != null)
                movementation.SetMouseSensitivity(PlayerPrefs.GetFloat("Mouse sensitivity"));

        }

        // Enables or disables weapon on the right side.
        public void SetRightSideWeapon(bool isOn)
        {
            PlayerPrefs.SetInt("RightSideWeapon", isOn ? 1 : 0);
            // Tries to update weapon position.
            PlayerWeaponController weaponController = FindObjectOfType<PlayerWeaponController>();
            if(weaponController != null)
                weaponController.SetRightSideWeapon(isOn);
                
        }

            //muda a resolucao pelo index selecionado
        public void SetResolution(int index)
        {
            Resolution newresolution = resolution[index];//seleciona com base no index
            Screen.SetResolution(newresolution.width, newresolution.height, Screen.fullScreen, newresolution.refreshRate); //aplica a nova resolucao
        }

        //entra e sai de FullScreen
        public void SetFullScreen(bool isOn)
        {
            Screen.fullScreen = isOn;
        }

        public void SetQuality(int qualityindex)//seleciona a qualidade de a cordo com o index atual aplicando ele no sistema base da unity
        {
            QualitySettings.SetQualityLevel(qualityindex);
        }

        //muda o volume
        public void SetVolume()
        {
            PlayerPrefs.SetFloat("Volume", volume.value);
            AudioListener.volume = volume.value;
        }

        // Enables or disables the speedrun HUD.
        public void SetSpeedrunHUD(bool isOn)
        {
            PlayerPrefs.SetInt("SpeedrunHUD", isOn ? 1 : 0);
            // Tries to update speedrun HUD status.
            HUDController hud = FindObjectOfType<HUDController>();
            if(hud != null)
                hud.Speedrun.SetEnabled(isOn);
        }

        // Enables or disables the statistics HUD.
        public void SetStatisticsHUD(bool isOn)
        {
            PlayerPrefs.SetInt("StatisticsHUD", isOn ? 1 : 0);
            // Tries to update statistics HUD status.
            HUDController hud = FindObjectOfType<HUDController>();
            if(hud != null)
                hud.Statistics.SetEnabled(isOn);
        }

    }
}

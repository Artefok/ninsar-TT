using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class colors_script_generator : MonoBehaviour
{
    [SerializeField] colors_scripts_materials colors;
    // private static string path = Application.dataPath + "/StreamingAssets/input_text.txt"; // путь к текстовому файлу
    public static string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "input_text.txt");
    private string input_text;
    private string res_color;
    private float pos_i;
    private float pos_j;
    private float timer;
    private bool on_cooldown = false;

    void Start()
    {
        pos_i = UnityEngine.Random.Range(0,8);
        pos_j = UnityEngine.Random.Range(0,9);
        input_text = System.IO.File.ReadAllText(filePath).ToString();
        Debug.Log(input_text);
        res_color = input_text.Split("\n")[Convert.ToInt32(pos_i)][Convert.ToInt32(pos_j)].ToString();
        gameObject.GetComponent<MeshRenderer>().material = colors.materials[Convert.ToInt32(res_color)-1];
        Debug.Log($"Cube #{gameObject.name}: {res_color} - {pos_i}:{pos_j}");
    }

    void Update()
    {
        int x = 0;
        int y = 0;
        if(!on_cooldown){
            if(Input.GetKeyDown(KeyCode.W)){
                x = -1;
            } else if(Input.GetKeyDown(KeyCode.S)){
                x = 1;
            }
            if(Input.GetKeyDown(KeyCode.A)){
                y = -1;
            } else if(Input.GetKeyDown(KeyCode.D)){
                y = 1;
            }

            pos_i += x;
            pos_j += y;
                
            if(pos_i > 8){
                pos_i = 0;
            } else if (pos_i < 0){
                pos_i = 8;
            }
            if(pos_j > 9){
                pos_j = 0;
            } else if(pos_j < 0){
                pos_j = 9;
            }

            if(x != 0 || y != 0){
                res_color = input_text.Split("\n")[Convert.ToInt32(pos_i)][Convert.ToInt32(pos_j)].ToString();
                gameObject.GetComponent<MeshRenderer>().material = colors.materials[Convert.ToInt32(res_color)-1];
                Debug.Log($"Cube #{gameObject.name}: {res_color} - {pos_i}:{pos_j}");
                x = 0;
                y = 0;
                on_cooldown = true;
            }
        } else {
            timer += Time.deltaTime;
            if(timer > 0.1f){
                on_cooldown = false;
                timer = 0;
            }
        }
    }
}

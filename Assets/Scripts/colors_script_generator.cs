using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class colors_script_generator : MonoBehaviour
{
    // Это скрипт одного куба, т.е. не общий генератор. У каждого свои значения переменных
    // Создан для генерации цветов в любых случаях

    [SerializeField] colors_scripts_materials colors; // цвета в виде списка материалов
    public static string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "input_text.txt"); // путь до файла
    private string input_text; // текст документа
    private string res_color; // итоговый цвет кубика
    private float pos_i; // позиция кубика в строке (индекс)
    private float pos_j; // позиция кубика в столбце (индекс)
    private float timer; // секунды таймера для кулдауна нажатия на кнопки
    private bool on_cooldown = false; // кулдаун нажатия на кнопки

    void Start()
    {
        // генерация позиции для последующего чтения документа
        pos_i = UnityEngine.Random.Range(0,8);
        pos_j = UnityEngine.Random.Range(0,9);

        // чтение документа
        input_text = System.IO.File.ReadAllText(filePath).ToString();

        // выставление нужного цвета кубику из позиции, сгенерированной ранее
        res_color = input_text.Split("\n")[Convert.ToInt32(pos_i)][Convert.ToInt32(pos_j)].ToString();
        gameObject.GetComponent<MeshRenderer>().material = colors.materials[Convert.ToInt32(res_color)-1];
    }

    void Update()
    {
        // обнуление/иницилизация показателей нажатия по осям X и Y/WASD (есть еще вариант с Input.GetAxis, но не додумался ранее использовать из-за float значений)
        int x = 0;
        int y = 0;

        if(!on_cooldown){ // проверка отсутствия кулдауна на нажатие клавиш

            // проверка нажатых клавиш
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

            // изменение основных позиций, в которых был прошлый цвет
            pos_i += x;
            pos_j += y;

            // проверка выхода основных позиций за рамки документа (оч тупая реализация, но на крайний случай можно воспользоваться и такой)                
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

            // проверка на наличие изменений в позициях
            if(x != 0 || y != 0){

                // выставление нужного цвета кубику из позиции, сгенерированной ранее
                res_color = input_text.Split("\n")[Convert.ToInt32(pos_i)][Convert.ToInt32(pos_j)].ToString();
                gameObject.GetComponent<MeshRenderer>().material = colors.materials[Convert.ToInt32(res_color)-1];

                // обнуление показателей нажатия по осям X и Y/WASD
                x = 0;
                y = 0;

                // начало кулдауна нажатия. в это время нельзя изменить позицию нажатием WASD
                on_cooldown = true;
            }
        } else { // если кулдаун появился
            // начало кулдауна нажатия по таймеру. в это время нельзя изменить позицию нажатием WASD
            timer += Time.deltaTime;
            if(timer > 0.1f){ // после того, как прошло 100 миллисекунд...
                //...кулдаун сбрасывается
                on_cooldown = false;
                timer = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PeerGrade
{
    /// <summary>
    /// Класс текст, содержит поля, применимые для все ткстовых файлов
    /// </summary>
    internal abstract class TextFile
    {
        private List<string> saves = new List<string>();
        private string name;
        private string path;
        private string  text;

        /// <summary>
        /// Текст из файла
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Имя и расширение файла
        /// </summary>
        internal string Name
        {
            get => name;
            
            set
            {
                name = Path.GetFileName(value);
            }
        }
        internal string FilePath
        {
            get => path;
        }

        /// <summary>
        /// Конструкстор класса
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        internal TextFile(string path)
        {
            this.path = path;
            Name = path;
            try
            {
                text = File.ReadAllText(path);
            }
            catch
            {
                
                File.Create(path).Close();
                text = File.ReadAllText(path);
            }
            
        }

        /// <summary>
        /// Соохранить файл
        /// </summary>
        public void Save()
        {
            if(saves.Count<15)
            {
            saves.Add(text);
            }
            else
            {
                saves.RemoveAt(0);
                saves.Add(text);
            }
        }
        

    }
}

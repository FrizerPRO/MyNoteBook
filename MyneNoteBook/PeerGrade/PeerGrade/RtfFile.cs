using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeerGrade
{
    /// <summary>
    /// Файлы с сохранением форматирования(*.rtf)
    /// </summary>
    internal class RtfFile:TextFile
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="path">путь к файлу</param>
        internal RtfFile(string path) : base(path)
        {

        }
        
    }
}

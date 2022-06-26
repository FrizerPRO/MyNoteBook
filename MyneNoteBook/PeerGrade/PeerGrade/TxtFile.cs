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
    /// Файлы не *.rtf (открываются как txt)
    /// </summary>
    internal class TxtFile:TextFile
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="path">путь к файлу</param>
        internal TxtFile(string path) : base(path)
        {

        }
        
    }
}

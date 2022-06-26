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
using System.Runtime.InteropServices;
using System.Threading;


namespace PeerGrade
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            CreateEmptyTxt(0);
            if (Properties.Settings.Default["allTabs"] != null)
                UseDeafaults();
            EnableIfSelected(false);
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(RichTextBox))
            {
                formatAllCode.Enabled = false;
                startTheCode.Enabled = false;
            }
            else if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
            {
                formatAllCode.Enabled = true;

                startTheCode.Enabled = true;
            }
        }

        /// <summary>
        /// Установка настроек как в закрытом окне
        /// </summary>
        private void UseDeafaults()
        {
            var tabs = (System.Collections.Specialized.StringCollection)Properties.Settings.Default["allTabs"];
            this.Size = (Size)Properties.Settings.Default["windowSize"];
            colorModeItem1.Checked = (bool)Properties.Settings.Default["blackColorMode"];
            timer1.Interval = (int)Properties.Settings.Default["timerMode"];
            int tabNumber = 0;
            foreach (var item in tabs)
            {
                TabPage tabPage = new TabPage($"Tab_{tabNumber++}.txt");
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.SelectionChanged += SelectionChanged;
                try
                {
                    richTextBox.Rtf = item;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Недопустимый формат файла.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Некая ошибка\nФайл может некорректно загрузиться из сохранений");
                }
                richTextBox.Size = tabPage.Size;
                richTextBox.Dock = DockStyle.Fill;
                richTextBox.ContextMenuStrip = contextMenuStrip1;
                tabPage.Controls.Add(richTextBox);
                tabControl1.TabPages.Add(tabPage);
            }
            if (colorModeItem1.Checked)
            {
                colorModeItem1.Checked = !colorModeItem1.Checked;
                ColorModeClick(new object(), EventArgs.Empty);
            }
        }

        /// <summary>
        /// Создать пустую вкладку
        /// </summary>
        /// <param name="tabNumber">Номер вкладки</param>
        private void CreateEmptyTxt(int tabNumber)
        {
            TxtFile txtFile = new TxtFile($"Tab_{tabNumber}.txt");
            openFileDialog1.FileName = $"Tab_{tabNumber}.txt";
            CreateTxtTab(txtFile);

        }

        /// <summary>
        /// Открывает текстовый файл
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void OpenTxtFile(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Текстовый файл (*.txt)|*.txt|Любой файл (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtFile txtFile = new TxtFile(openFileDialog1.FileName);
                CreateTxtTab(txtFile);
            }
            if (colorModeItem1.Checked)
            {
                colorModeItem1.Checked = !colorModeItem1.Checked;
                ColorModeClick(sender, e);
            }
        }

        /// <summary>
        /// Создает вкладку с текстовым файлом
        /// </summary>
        /// <param name="txtFile"></param>
        private void CreateTxtTab(TxtFile txtFile)
        {
            TabPage tabPage = new TabPage(txtFile.Name);
            tabPage.Tag = txtFile.FilePath;
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.SelectionChanged += SelectionChanged;
            richTextBox.Text = txtFile.Text;
            richTextBox.Size = tabPage.Size;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ContextMenuStrip = contextMenuStrip1;
            tabPage.Controls.Add(richTextBox);
            tabControl1.TabPages.Add(tabPage);
        }

        /// <summary>
        /// Открывает текстовый файл Rtf
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void OpenRtfFile(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Текстовый файл (*.rtf)|*.rtf";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                RtfFile rtfFile = new RtfFile(openFileDialog1.FileName);
                CreateRtfTab(rtfFile);
            }
            if (colorModeItem1.Checked)
            {
                colorModeItem1.Checked = !colorModeItem1.Checked;
                ColorModeClick(sender, e);
            }

        }

        /// <summary>
        /// Создает вкладку с текстовым файлом Rtf
        /// </summary>
        /// <param name="rtfFile"></param>
        private void CreateRtfTab(RtfFile rtfFile)
        {
            TabPage tabPage = new TabPage(rtfFile.Name);
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.SelectionChanged += SelectionChanged;
            
            try
            {
                File.Create("timeFile.txt").Close();
                File.WriteAllText("timeFile.txt", rtfFile.Text);
                richTextBox.LoadFile("timeFile.txt");
                File.Delete("timeFile.txt");

            }
            catch (ArgumentException)
            {
                MessageBox.Show("Недопустимый формат файла.");
            }
            catch (Exception)
            {
                MessageBox.Show("Некая ошибка, попробуйте другой файл");
            }
            richTextBox.Size = tabPage.Size;
            richTextBox.Dock = DockStyle.Fill;

            //  tabPage.Text = richTextBox.Text;
            richTextBox.ContextMenuStrip = contextMenuStrip1;
            tabPage.Controls.Add(richTextBox);
            tabControl1.TabPages.Add(tabPage);
        }

        /// <summary>
        /// Сохраняет текстовый файл
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveTxtFile(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовый файл (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, tabControl1.SelectedTab.Controls[0].Text);
            }

        }

        /// <summary>
        /// Сохраняет текстовый файл Rtf
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveRtfFile(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовый файл (*.rtf)|*.rtf";
            RichTextBox r = (RichTextBox)tabControl1.SelectedTab.Controls[0];
            r.SelectAll();
            r.SelectionColor = Color.Black;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                r.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichNoOleObjs);
            }
        }

        /// <summary>
        /// Проверяет корректность значения размер текста(от 1 до 255 включительно)
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void TextSizeParse(object sender, EventArgs e)
        {
            byte size;
            if ((byte.TryParse(toolStripTextBox1.Text, out size)) && (size > 0))
            {

            }
            else
            {
                toolStripTextBox1.Text = "";
                MessageBox.Show("Вводите Размер шрифта корректно!\nМаксимальный размер-255");
            }

        }

        /// <summary>
        /// Выбирает параметры форматирования
        /// </summary>
        /// <param name="selectMode"></param>
        private void EnableIfSelected(bool selectMode)
        {
            italicCheckBox.Enabled = selectMode;
            boldCheckBox.Enabled = selectMode;
            underlinedCheckBox.Enabled = selectMode;
            linedCheckBox.Enabled = selectMode;
            italicToolStripMenuItem.Enabled = italicCheckBox.Enabled;
            linedToolStripMenuItem.Enabled = linedCheckBox.Enabled;
            underlinedToolStripMenuItem.Enabled = underlinedCheckBox.Enabled;
            boldToolStripMenuItem.Enabled = boldCheckBox.Enabled;
            copyToolStripMenuItem.Enabled = selectMode;
            cutToolStripMenuItem.Enabled = selectMode;
            italicToolStripMenuItem.Checked = italicCheckBox.Checked;
            linedToolStripMenuItem.Checked = linedCheckBox.Checked;
            underlinedToolStripMenuItem.Checked = underlinedCheckBox.Checked;
            boldToolStripMenuItem.Checked = boldCheckBox.Checked;
            timesNewRomanToolStripMenuItem.Enabled = selectMode;
            futuraToolStripMenuItem.Enabled = selectMode;
            arialToolStripMenuItem.Enabled = selectMode;
            toolStripTextBox1.Enabled = selectMode;

        }

        /// <summary>
        /// Изменяет индикаторы форматирования в зависимости от выбранного текста
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SelectionChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                return;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectedText.Length == 0)
            {

                italicCheckBox.Checked = false;
                boldCheckBox.Checked = false;
                underlinedCheckBox.Checked = false;
                linedCheckBox.Checked = false;
                EnableIfSelected(false);
                return;
            }
            EnableIfSelected(true);
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style.HasFlag(FontStyle.Italic))
            {
                italicCheckBox.Checked = true;
            }
            else italicCheckBox.Checked = false;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style.HasFlag(FontStyle.Bold))
            {
                boldCheckBox.Checked = true;
            }
            else boldCheckBox.Checked = false;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style.HasFlag(FontStyle.Underline))
            {
                underlinedCheckBox.Checked = true;
            }
            else underlinedCheckBox.Checked = false;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style.HasFlag(FontStyle.Strikeout))
            {
                linedCheckBox.Checked = true;
            }
            else linedCheckBox.Checked = false;
            italicToolStripMenuItem.Checked = italicCheckBox.Checked;
            linedToolStripMenuItem.Checked = linedCheckBox.Checked;
            underlinedToolStripMenuItem.Checked = underlinedCheckBox.Checked;
            boldToolStripMenuItem.Checked = boldCheckBox.Checked;
        }
        private void SelectionInCodeChanged(object sender, EventArgs e)
        {
            EnableIfSelected(false);
        }

        /// <summary>
        /// Проверяет корректность данных при изменении форматирования
        /// </summary>
        /// <returns></returns>
        private bool IsSelected()
        {
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return false;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectedText.Length == 0)
            {
                MessageBox.Show("Сначала выделите текст!");
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Изменятеся при нажатии на Курсив
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ItalicCheckBoxClick(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                if (italicCheckBox.Checked)
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Italic | ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);
                }
                else
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Italic ^ ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);

                }
            }
            else italicCheckBox.Checked = !italicCheckBox.Checked;
        }

        /// <summary>
        /// Изменятеся при нажатии на Жирный
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void BoldCheckBoxClick(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                if (boldCheckBox.Checked)
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Bold | ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);
                }
                else
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Bold ^ ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);
                }
            }
            else boldCheckBox.Checked = !boldCheckBox.Checked;
        }

        /// <summary>
        /// Изменятеся при нажатии на Подчеркнутый
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void UnderlinedCheckBoxClick(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                if (underlinedCheckBox.Checked)
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Underline | ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);

                }
                else
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Underline ^ ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);

                }
            }
            else underlinedCheckBox.Checked = !underlinedCheckBox.Checked;
        }

        /// <summary>
        /// Изменятеся при нажатии на Зачеркнутый
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void LinedCheckBoxClick(object sender, EventArgs e)
        {

            if (IsSelected())
            {
                if (linedCheckBox.Checked)
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Strikeout | ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);
                }
                else
                {
                    ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont, FontStyle.Strikeout ^ ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.Style);
                }

            }
            else linedCheckBox.Checked = !linedCheckBox.Checked;
        }

        /// <summary>
        /// Изменятеся при нажатии на Курсив
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ItalianContextMenu(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                italicCheckBox.Checked = !italicCheckBox.Checked;
                ItalicCheckBoxClick(sender, e);
                italicToolStripMenuItem.Checked = italicCheckBox.Checked;


            }
        }

        /// <summary>
        /// Изменятеся при нажатии на Жирный
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void BoldContextMenu(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                boldCheckBox.Checked = !boldCheckBox.Checked;
                BoldCheckBoxClick(sender, e);

                boldToolStripMenuItem.Checked = boldCheckBox.Checked;

            }
        }

        /// <summary>
        /// Изменятеся при нажатии на Подчеркнутый
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void UnderlinedContextMenu(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                underlinedCheckBox.Checked = !underlinedCheckBox.Checked;
                UnderlinedCheckBoxClick(sender, e);

                underlinedToolStripMenuItem.Checked = underlinedCheckBox.Checked;
            }
        }

        /// <summary>
        /// Изменятеся при нажатии на Зачеркнутый
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void LinedContextMenu(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                linedCheckBox.Checked = !linedCheckBox.Checked;
                LinedCheckBoxClick(sender, e);
                linedToolStripMenuItem.Checked = linedCheckBox.Checked;
            }
        }

        /// <summary>
        /// Выбрать весь текст через контекстное меню
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SelectAllTextToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(RichTextBox))
                ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectAll();
            else if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                ((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).SelectAll();
        }

        /// <summary>
        /// Копировать текст
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                ((RichTextBox)tabControl1.SelectedTab.Controls[0]).Copy();
            }
        }

        /// <summary>
        /// Вставить текст
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(RichTextBox))
                ((RichTextBox)tabControl1.SelectedTab.Controls[0]).Paste();
            else if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                ((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).Paste();

        }

        /// <summary>
        /// Вырезать текст
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (IsSelected())
            {
                ((RichTextBox)tabControl1.SelectedTab.Controls[0]).Cut();
            }
        }

        /// <summary>
        /// Закрыть форму
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void Form1Closing(object sender, FormClosingEventArgs e)
        {

            int numberOfTabs = tabControl1.TabPages.Count;
            foreach (TabPage item in tabControl1.TabPages)
            {
                
                tabControl1.SelectedTab = item;
                if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(RichTextBox))
                {
                    if (((openFileDialog1.FileName == string.Empty) || (((RichTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(openFileDialog1.FileName)))
                        && ((saveFileDialog1.FileName == string.Empty) || (((RichTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(saveFileDialog1.FileName))))
                    {
                        SaveNoCodeAs(sender, e, item);
                    }
                }
                else if (tabControl1.SelectedTab.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                {
                    if (((openFileDialog1.FileName == string.Empty) || (((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(openFileDialog1.FileName)))
                        && ((saveFileDialog1.FileName == string.Empty) || (((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(saveFileDialog1.FileName))))
                    {
                        SaveCodeAs(sender, e, item);
                    }
                }
            }
            SetDefaults();
        }

        private void SaveNoCodeAs(object sender, FormClosingEventArgs e, object item)
        {
            DialogResult result = MessageBox.Show($"Сохранить изменения?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                DialogResult wayOfSave = MessageBox.Show($"Сохранить форматирование?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
                {
                    switch (wayOfSave)
                    {
                        case DialogResult.Yes:
                            SaveRtfFile(sender, e);
                            break;
                        case DialogResult.No:
                            SaveTxtFile(sender, e);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void SaveCodeAs(object sender, FormClosingEventArgs e, object item)
        {
            DialogResult result = MessageBox.Show($"Сохранить изменения?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                DialogResult wayOfSave = MessageBox.Show($"Сохранить форматирование?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
                {
                    switch (wayOfSave)
                    {

                        case DialogResult.Yes:
                            SaveRtfFile(sender, e);
                            break;
                        case DialogResult.No:
                            SaveTxtFile(sender, e);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Установить текущие настройки как дефолтныые
        /// </summary>
        private void SetDefaults()
        {
            System.Collections.Specialized.StringCollection tabs = new System.Collections.Specialized.StringCollection();
            foreach (TabPage item in tabControl1.TabPages)
            {
                if (item.Controls[0].GetType() == typeof(RichTextBox))
                {
                    RichTextBox r = (RichTextBox)item.Controls[0];
                    r.SelectAll();
                    r.SelectionColor = Color.Black;
                    tabs.Add(((RichTextBox)item.Controls[0]).Rtf);
                }
                else if (item.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                {
                    FastColoredTextBoxNS.FastColoredTextBox r = (FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0];
                    r.SelectAll();
                    r.SelectionColor = Color.Black;
                    tabs.Add(((FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0]).Rtf);
                }

            }
            Properties.Settings.Default["allTabs"] = tabs;
            Properties.Settings.Default["windowSize"] = this.Size;
            Properties.Settings.Default["blackColorMode"] = colorModeItem1.Checked;
            Properties.Settings.Default["timerMode"] = timer1.Interval;
            Properties.Settings.Default.Save();

        }

        /// <summary>
        ///  отменить
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void UndoClick(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl1.SelectedTab.Controls[0]).Undo();
        }

        /// <summary>
        ///  Вернуть
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ReundoClick(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl1.SelectedTab.Controls[0]).Redo();
        }

        /// <summary>
        /// Скопировать
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CopyMenuButton(object sender, EventArgs e)
        {
            CopyToolStripMenuItemClick(sender, e);
        }

        /// <summary>
        /// Вставить
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void PasteMenuButton(object sender, EventArgs e)
        {
            PasteToolStripMenuItemClick(sender, e);
        }

        /// <summary>
        /// Вырезать
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CutMenuButton(object sender, EventArgs e)
        {
            CutToolStripMenuItemClick(sender, e);
        }

        /// <summary>
        /// Шрифт на Таймс
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void TimesNewRomanClick(object sender, EventArgs e)
        {
            int size;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return;
            int.TryParse(toolStripTextBox1.Text, out size);
            ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font("Times New Roman", size);

        }

        /// <summary>
        /// Шрифт на Футура
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void FuturaClick(object sender, EventArgs e)
        {
            int size;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return;
            int.TryParse(toolStripTextBox1.Text, out size);
            ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font("Futura", size);
        }

        /// <summary>
        /// Шрифт Ария
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ArialClick(object sender, EventArgs e)
        {
            int size;
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return;
            int.TryParse(toolStripTextBox1.Text, out size);
            ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font("Arial", size);
        }

        /// <summary>
        /// Если изменился размер текста
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void TextSizeChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont == null) return;
            int size;
            if (int.TryParse(toolStripTextBox1.Text, out size))
            {
                ((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont = new Font(((RichTextBox)tabControl1.SelectedTab.Controls[0]).SelectionFont.FontFamily, size);
            }
        }

        /// <summary>
        /// Созранить этот файл
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveCurrentClick(object sender, EventArgs e)
        {
            DialogResult wayOfSave = MessageBox.Show("Сохранить форматирование?", "Question", MessageBoxButtons.YesNo);
            {
                switch (wayOfSave)
                {

                    case DialogResult.Yes:
                        SaveRtfFile(sender, e);
                        break;
                    case DialogResult.No:
                        SaveTxtFile(sender, e);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Сохранить все
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveAllClick(object sender, EventArgs e)
        {
            int numberOfTabs = tabControl1.TabPages.Count;
            foreach (var item in tabControl1.TabPages)
            {
                tabControl1.SelectedTab = (TabPage)item;
                DialogResult wayOfSave = MessageBox.Show($"Сохранить форматирование?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
                {
                    switch (wayOfSave)
                    {
                        case DialogResult.Yes:
                            SaveRtfFile(sender, e);
                            break;
                        case DialogResult.No:
                            SaveTxtFile(sender, e);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Создать новую вкладку
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void InThisWindowClick(object sender, EventArgs e)
        {

            CreateEmptyTxt(tabControl1.TabPages.Count);
            if (colorModeItem1.Checked)
                ColorModeClick(sender, e);
        }

        /// <summary>
        /// Создать новую вкладку в новом окне
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void InNewWindowClick(object sender, EventArgs e)
        {
            SetDefaultDefaults();
            (new Form1()).Show();
            if (colorModeItem1.Checked)
                ColorModeClick(sender, e);
        }

        /// <summary>
        /// Установить Настройки из коробки(нулевые)
        /// </summary>
        private void SetDefaultDefaults()
        {
            Properties.Settings.Default["allTabs"] = null;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Закрыть окно
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CloseWindowClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Закрыть вкладку
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CloseTabToolStripMenuItemClick(object sender, EventArgs e)
        {
            var item = tabControl1.SelectedTab;
            if (((openFileDialog1.FileName == string.Empty) || (((RichTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(openFileDialog1.FileName)))
                && ((saveFileDialog1.FileName == string.Empty) || (((RichTextBox)tabControl1.SelectedTab.Controls[0]).Rtf != File.ReadAllText(saveFileDialog1.FileName))))
            {
                DialogResult result = MessageBox.Show($"Сохранить изменения?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DialogResult wayOfSave = MessageBox.Show($"Сохранить форматирование?\nДля {((TabPage)item).Text}", "Question", MessageBoxButtons.YesNo);
                    {
                        switch (wayOfSave)
                        {

                            case DialogResult.Yes:
                                SaveRtfFile(sender, e);
                                break;
                            case DialogResult.No:
                                SaveTxtFile(sender, e);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        /// <summary>
        /// Сохранять каждую минуту
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveEveryMinuteClick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 60000;
        }

        /// <summary>
        /// Сохранять каждые 5 минут
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveEvery5MinuteClick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 300000;
        }

        /// <summary>
        /// Сохранять каждые 10 минут
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveEvery10MinutesClick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 600000;
        }

        /// <summary>
        /// Сохранять каждый час
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void SaveEveryHourClick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 3600000;
        }

        /// <summary>
        /// Сохранение по таймеру
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void timerTick(object sender, EventArgs e)
        {
            SaveAllClick(sender, e);
        }

        /// <summary>
        /// Не сохранять
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void DoNotSave(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        /// <summary>
        /// Выбор цветовой темы
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ColorModeClick(object sender, EventArgs e)
        {
            colorModeItem1.Checked = !colorModeItem1.Checked;
            if (colorModeItem1.Checked)
            {
                foreach (TabPage item in tabControl1.TabPages)
                {
                    ChangeColorBack(item);

                }
                this.BackColor = Color.Black;
            }
            else
            {
                foreach (TabPage item in tabControl1.TabPages)
                {
                    if (item.Controls[0].GetType() == typeof(RichTextBox))
                    {
                        item.Controls[0].BackColor = Color.White;
                        ((RichTextBox)item.Controls[0]).SelectAll();
                        ((RichTextBox)item.Controls[0]).SelectionColor = Color.Black;
                    }
                    else if (item.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
                    {
                        item.Controls[0].BackColor = Color.White;
                        ((FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0]).SelectAll();
                        ((FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0]).SelectionColor = Color.Black;
                    }
                }
                this.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Меняет цвет вкладки на черный или на белый
        /// </summary>
        /// <param name="item">Номер вкладки</param>
        private static void ChangeColorBack(TabPage item)
        {
            if (item.Controls[0].GetType() == typeof(RichTextBox))
            {
                item.Controls[0].BackColor = Color.Black;
                ((RichTextBox)item.Controls[0]).SelectAll();
                ((RichTextBox)item.Controls[0]).SelectionColor = Color.White;
            }
            else if (item.Controls[0].GetType() == typeof(FastColoredTextBoxNS.FastColoredTextBox))
            {
                item.Controls[0].BackColor = Color.Black;
                ((FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0]).SelectAll();
                ((FastColoredTextBoxNS.FastColoredTextBox)item.Controls[0]).SelectionColor = Color.White;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void OpenCode(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Код на языке C# (*.cs)|*.cs";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtFile txtFile = new TxtFile(openFileDialog1.FileName);
                CreateCodeTab(txtFile);
            }
            if (colorModeItem1.Checked)
            {
                colorModeItem1.Checked = !colorModeItem1.Checked;
                ColorModeClick(sender, e);
            }
        }

        /// <summary>
        /// Создать вкладку .cs
        /// </summary>
        /// <param name="txtFile"></param>
        private void CreateCodeTab(TxtFile txtFile)
        {
            TabPage tabPage = new TabPage(txtFile.Name);
            tabPage.Tag = "cs";
            FastColoredTextBoxNS.FastColoredTextBox coloredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            coloredTextBox.SelectionChanged += SelectionInCodeChanged;
            coloredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
            coloredTextBox.Text = txtFile.Text;
            coloredTextBox.Size = tabPage.Size;
            coloredTextBox.Dock = DockStyle.Fill;
            coloredTextBox.ContextMenuStrip = contextMenuStrip1;
            tabPage.Controls.Add(coloredTextBox);
            tabControl1.TabPages.Add(tabPage);
            EnableIfSelected(false);
        }

        /// <summary>
        /// Сохранить .cs файл
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void saveCodeFile(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Файл C# (*.cs)|*.cs";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, tabControl1.SelectedTab.Controls[0].Text);
            }
        }

        /// <summary>
        /// Создать пустой .cs вкладка
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void CreateEmptyCode(object sender, EventArgs e)
        {
            TxtFile txtFile = new TxtFile($"Tab_{tabControl1.TabCount}.cs");
            openFileDialog1.FileName = $"Tab_{tabControl1.TabCount}_code.cs";
            File.Create(openFileDialog1.FileName).Close();
            CreateCodeTab(txtFile);
        }

        /// <summary>
        /// Отформатировать код
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void FormatCode(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).SelectAll();
            ((FastColoredTextBoxNS.FastColoredTextBox)tabControl1.SelectedTab.Controls[0]).DoAutoIndent();
        }

        /// <summary>
        /// Запустить код
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void startTheCode_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "Файл C# (*.cs)|*.cs";
            if (saveFileDialog2.FileName == string.Empty)
            {
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {

                }
            }
            File.WriteAllText(saveFileDialog2.FileName, tabControl1.SelectedTab.Controls[0].Text);
            System.Diagnostics.Process.Start("CMD.exe", $"/C c:\\windows\\Microsoft.NET\\Framework\\v3.5\\csc.exe /t:exe /out:{saveFileDialog2.FileName}.exe {saveFileDialog2.FileName} > stream.txt");
            DateTime d = DateTime.Now;
            while (!File.Exists($"{saveFileDialog2.FileName}.exe"))
            {
                if (d.AddSeconds(5) < DateTime.Now) break;
            }
            if (File.Exists($"{saveFileDialog2.FileName}.exe"))
                System.Diagnostics.Process.Start("CMD.exe", $"/K {saveFileDialog2.FileName}.exe");
            else
            {

                MessageBox.Show(File.ReadAllText("stream.txt"), "Compile error");
            }

        }

        /// <summary>
        /// Изменить путь к скомпилированному файлу
        /// </summary>
        /// <param name="sender">Ссылка на элемент, вызвавший событие</param>
        /// <param name="e">Данные о событии</param>
        private void ChangeCompilePath(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "Файл C# (*.cs)|*.cs";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}



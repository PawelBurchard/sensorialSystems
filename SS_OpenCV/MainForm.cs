using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace SS_OpenCV
{
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // imagem corrente
        Image<Bgr, Byte> imgUndo = null; // imagem backup - UNDO
        string title_bak = "";

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Abrir uma nova imagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Guardar a imagem com novo nome
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Fecha a aplicação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// repoe a ultima copia da imagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // protege de executar a função sem ainda ter aberto a imagem 
                return; 
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();
            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Altera o modo de vizualização
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // com scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Mostra a janela Autores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }


        /// <summary>
        /// Converte a imagem para tons de cinzento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        /// <summary>
        /// Efectua o negativo da imagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToNegative(img);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();
            InputBox formx = new InputBox();
            formx.ShowDialog();
            InputBox formy = new InputBox();
            formy.ShowDialog();
            ImageClass.Translate(img, Convert.ToInt16(formx.getValue), Convert.ToInt16(formy.getValue));

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            InputBox form = new InputBox();
            form.ShowDialog();

            ImageClass.Rotate(img, form.getValue);
            
            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            InputBox form = new InputBox();
            form.ShowDialog();

            ImageClass.Scale(img, form.getValue);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void noiseReductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.NoiseReduction(img, 1, 1, 1, 1, 1, 1, 1, 1, 1, 9);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void effectsFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            weightMatrix form = new weightMatrix();
            form.ShowDialog();
            
            if(!form.Cancel)
                ImageClass.NoiseReduction(img, form.WeightMtx[0], form.WeightMtx[1], form.WeightMtx[2], form.WeightMtx[3], form.WeightMtx[4], form.WeightMtx[5], form.WeightMtx[6], form.WeightMtx[7], form.WeightMtx[8], form.Weight1 );

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void robertsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Roberts(img);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Median(img);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // protege de executar a função sem ainda ter aberto a imagem 
                return;
            Cursor = Cursors.WaitCursor; // cursor relogio

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Sobel(img);

            ImageViewer.Refresh(); // atualiza imagem no ecrã

            Cursor = Cursors.Default; // cursor normal
        }



    }
}
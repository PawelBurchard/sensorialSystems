using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Linq;


namespace SS_OpenCV
{
    class ImageClass
    {
       
        internal static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int x, y;
                        
                if (nChan == 3) // imagem em RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // converte para cinza
                            gray = (byte)(((int)blue + green + red) / 3);

                            // guarda na imagem
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // avança apontador para próximo pixel
                            dataPtr += nChan;
                        }

                        //no fim da linha avança alinhamento (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        internal static void ConvertToNegative(Image<Bgr, byte> img)
        {
            unsafe
            {


                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int x, y;

                if (nChan == 3) // imagem em RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            
                            // guarda na imagem
                            dataPtr[0] = (byte)(255 - (int)dataPtr[0]);
                            dataPtr[1] = (byte)(255 - (int)dataPtr[1]);
                            dataPtr[2] = (byte)(255 - (int)dataPtr[2]);

                            // avança apontador para próximo pixel
                            dataPtr += nChan;
                        }

                        //no fim da linha avança alinhamento (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        internal static void Translate(Image<Bgr, byte> img, int movex, int movey)
        {

            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

                byte* dataPtrpom = dataPtr;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    { 
                         if (x < movex)
                         {
                             dataPtr[0] = 0;
                             dataPtr[1] = 0;
                             dataPtr[2] = 0;
                         }
                     
                         if (y < movey)
                         {
                             dataPtr[0] = 0;
                             dataPtr[1] = 0;
                             dataPtr[2] = 0;
                         }
                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

                dataPtr = dataPtrpom;
                for (int i = 0; i < movey; i++)
                {
                    dataPtr += nChan*width;
                    dataPtr += padding;
                }
                dataPtr += nChan*movex;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    { 
                        dataPtr[0] = dataPtr2[0];
                        dataPtr[1] = dataPtr2[1];
                        dataPtr[2] = dataPtr2[2];

                        dataPtr2 += nChan2;

                        if(x<width-movex)
                            dataPtr += nChan;
                    }
                    
                    dataPtr2 += padding2;
                    dataPtr += padding;
                    dataPtr += nChan*movex;
                }
                
            }
        }

        internal static void Rotate(Image<Bgr, byte> img, double Angle)
        {

            unsafe
            {
                double angle = Angle*Math.PI/180;
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

                byte* dataPtrpom = dataPtr;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        dataPtr[0] = 0;
                        dataPtr[1] = 0;
                        dataPtr[2] = 0;
                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        int newx = Convert.ToInt16(Math.Cos(angle) * x + Math.Sin(angle) * y);
                        int newy = Convert.ToInt16(-Math.Sin(angle) * x + Math.Cos(angle) * y);

                        dataPtr = dataPtrpom;
                        if (newx < width & newy < height)
                        {
                            for (int i = 0; i < newy; i++)
                            {
                                dataPtr += nChan * width;
                                dataPtr += padding;
                            }
                            dataPtr += nChan * newx;

                            dataPtr[0] = dataPtr2[0];
                            dataPtr[1] = dataPtr2[1];
                            dataPtr[2] = dataPtr2[2];
                        }
                        dataPtr2 += nChan2;
                    }
                    dataPtr2 += padding2;
                }
            }
        }

        internal static void Scale(Image<Bgr, byte> img, double scale)
        {

            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

                byte* dataPtrpom = dataPtr;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        dataPtr[0] = 0;
                        dataPtr[1] = 0;
                        dataPtr[2] = 0;
                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        int newx = Convert.ToInt16(x*scale);
                        int newy = Convert.ToInt16(y*scale);

                        dataPtr = dataPtrpom;
                        if (newx < width & newy < height)
                        {
                            for (int i = 0; i < newy; i++)
                            {
                                dataPtr += nChan * width;
                                dataPtr += padding;
                            }
                            dataPtr += nChan * newx;

                            dataPtr[0] = dataPtr2[0];
                            dataPtr[1] = dataPtr2[1];
                            dataPtr[2] = dataPtr2[2];
                        }
                        dataPtr2 += nChan2;
                    }
                    dataPtr2 += padding2;
                }
            }
        }

        /*this is the function to be checked*/
        internal static void NoiseReduction(Image<Bgr, byte> img, int w11, int w12, int w13, int w21, int w22, int w23, int w31, int w32, int w33, double wmain)
        {

            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int widthStep = m.widthStep;
                int widthStep2 = m2.widthStep;
                int width2 = imgcopy.Width;
                int height2 = imgcopy.Height;

                byte* dataPtrpom = dataPtr;
                byte* dataPtrpom2 = dataPtr2;

                int[] pixels = new int[3];

                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;

                //centre
                dataPtr += widthStep + nChan;
                dataPtr2 += widthStep2 + nChan2;

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        pixels[0] += (dataPtr2 - widthStep2 - nChan2)[0] * w11;
                        pixels[1] += (dataPtr2 - widthStep2 - nChan2)[1] * w11;
                        pixels[2] += (dataPtr2 - widthStep2 - nChan2)[2] * w11;
                        pixels[0] += (dataPtr2 - widthStep2)[0] * w12;
                        pixels[1] += (dataPtr2 - widthStep2)[1] * w12;
                        pixels[2] += (dataPtr2 - widthStep2)[2] * w12;
                        pixels[0] += (dataPtr2 - widthStep2 + nChan2)[0] * w13;
                        pixels[1] += (dataPtr2 - widthStep2 + nChan2)[1] * w13;
                        pixels[2] += (dataPtr2 - widthStep2 + nChan2)[2] * w13;
                        pixels[0] += (dataPtr2 - nChan2)[0] * w21;
                        pixels[1] += (dataPtr2 - nChan2)[1] * w21;
                        pixels[2] += (dataPtr2 - nChan2)[2] * w21;
                        pixels[0] += dataPtr2[0] * w22;
                        pixels[1] += dataPtr2[1] * w22;
                        pixels[2] += dataPtr2[2] * w22;
                        pixels[0] += (dataPtr2 + nChan2)[0] * w23;
                        pixels[1] += (dataPtr2 + nChan2)[1] * w23;
                        pixels[2] += (dataPtr2 + nChan2)[2] * w23;
                        pixels[0] += (dataPtr2 + widthStep2 - nChan2)[0] * w31;
                        pixels[1] += (dataPtr2 + widthStep2 - nChan2)[1] * w31;
                        pixels[2] += (dataPtr2 + widthStep2 - nChan2)[2] * w31;
                        pixels[0] += (dataPtr2 + widthStep2)[0] * w32;
                        pixels[1] += (dataPtr2 + widthStep2)[1] * w32;
                        pixels[2] += (dataPtr2 + widthStep2)[2] * w32;
                        pixels[0] += (dataPtr2 + widthStep2 + nChan2)[0] * w33;
                        pixels[1] += (dataPtr2 + widthStep2 + nChan2)[1] * w33;
                        pixels[2] += (dataPtr2 + widthStep2 + nChan2)[2] * w33;

                        pixels[0] = Convert.ToInt32(pixels[0] / wmain);
                        pixels[1] = Convert.ToInt32(pixels[1] / wmain);
                        pixels[2] = Convert.ToInt32(pixels[2] / wmain);
                        if (pixels[0] > 255)
                            pixels[0] = 255;
                        if (pixels[1] > 255)
                            pixels[1] = 255;
                        if (pixels[2] > 255)
                            pixels[2] = 255;
                        if (pixels[0] < 0)
                            pixels[0] = 0;
                        if (pixels[1] < 0)
                            pixels[1] = 0;
                        if (pixels[2] < 0)
                            pixels[2] = 0;
                        dataPtr[0] = Convert.ToByte(pixels[0]);
                        dataPtr[1] = Convert.ToByte(pixels[1]);
                        dataPtr[2] = Convert.ToByte(pixels[2]);
                        pixels[0] = 0;
                        pixels[1] = 0;
                        pixels[2] = 0;
                        dataPtr += nChan;
                        dataPtr2 += nChan2;
                    }
                    dataPtr += 2 * nChan + padding;
                    dataPtr2 +=  2 * nChan2 + padding2;
                }
                
                
                dataPtr = dataPtrpom;
                dataPtr2 = dataPtrpom2;

                int[] sum = new int[3]; //left top corner 
                sum[0] += dataPtr2[0] * (w11 + w12 + w21 + w22);
                sum[1] += dataPtr2[1] * (w11 + w12 + w21 + w22);
                sum[2] += dataPtr2[2] * (w11 + w12 + w21 + w22);
                dataPtr2 += nChan2;
                sum[0] += dataPtr2[0] * (w13 + w23);
                sum[1] += dataPtr2[2] * (w13 + w23);
                sum[2] += dataPtr2[2] * (w13 + w23);
                dataPtr2 += widthStep;
                sum[0] += dataPtr2[0] * (w31 + w32);
                sum[1] += dataPtr2[1] * (w31 + w32);
                sum[2] += dataPtr2[2] * (w31 + w32);
                dataPtr2 -= nChan2;
                sum[0] += dataPtr2[0] * w33;
                sum[1] += dataPtr2[1] * w33;
                sum[2] += dataPtr2[2] * w33;

                sum[0] = Convert.ToInt32(sum[0] / wmain);
                sum[1] = Convert.ToInt32(sum[1] / wmain);
                sum[2] = Convert.ToInt32(sum[2] / wmain);
                if (sum[0] > 255)
                    sum[0] = 255;
                if (sum[1] > 255)
                    sum[1] = 255;
                if (sum[2] > 255)
                    sum[2] = 255;
                if (sum[0] < 0)
                    sum[0] = 0;
                if (sum[1] < 0)
                    sum[1] = 0;
                if (sum[2] < 0)
                    sum[2] = 0;
                dataPtr[0] = Convert.ToByte(sum[0]);
                dataPtr[1] = Convert.ToByte(sum[1]);
                dataPtr[2] = Convert.ToByte(sum[2]);

                dataPtr += nChan;

                sum[0] = 0;
                sum[1] = 0;
                sum[2] = 0;

                dataPtr2 = dataPtrpom2;

                //top
                for (int i = 0; i < width - 2; i++)
                {
                    sum[0] += dataPtr2[0] * (w11 + w21);
                    sum[1] += dataPtr2[1] * (w11 + w21);
                    sum[2] += dataPtr2[2] * (w11 + w21);
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * (w12 + w22);
                    sum[1] += dataPtr2[1] * (w12 + w22);
                    sum[2] += dataPtr2[2] * (w12 + w22);
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * (w13 + w23);
                    sum[1] += dataPtr2[1] * (w13 + w23);
                    sum[2] += dataPtr2[2] * (w13 + w23);
                    dataPtr2 += widthStep - 2 * nChan2;
                    sum[0] += dataPtr2[0] * w31;
                    sum[1] += dataPtr2[1] * w31;
                    sum[2] += dataPtr2[2] * w31;
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * w32;
                    sum[1] += dataPtr2[1] * w32;
                    sum[2] += dataPtr2[2] * w32;
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * w33;
                    sum[1] += dataPtr2[1] * w33;
                    sum[2] += dataPtr2[2] * w33;
                    dataPtr2 += -widthStep - nChan2;

                    sum[0] = Convert.ToInt32(sum[0] / wmain);
                    sum[1] = Convert.ToInt32(sum[1] / wmain);
                    sum[2] = Convert.ToInt32(sum[2] / wmain);
                    if (sum[0] > 255)
                        sum[0] = 255;
                    if (sum[1] > 255)
                        sum[1] = 255;
                    if (sum[2] > 255)
                        sum[2] = 255;
                    if (sum[0] < 0)
                        sum[0] = 0;
                    if (sum[1] < 0)
                        sum[1] = 0;
                    if (sum[2] < 0)
                        sum[2] = 0;
                    dataPtr[0] = Convert.ToByte(sum[0]);
                    dataPtr[1] = Convert.ToByte(sum[1]);
                    dataPtr[2] = Convert.ToByte(sum[2]);

                    dataPtr += nChan;

                    sum[0] = 0;
                    sum[1] = 0;
                    sum[2] = 0;
                }

                //right top corner 
                dataPtr2 += nChan2;

                sum[0] += dataPtr2[0] * (w12 + w13 + w22 + w23);
                sum[1] += dataPtr2[1] * (w12 + w13 + w22 + w23);
                sum[2] += dataPtr2[2] * (w12 + w13 + w22 + w23);
                dataPtr2 -= nChan2;
                sum[0] += dataPtr2[0] * (w11 + w21);
                sum[1] += dataPtr2[2] * (w11 + w21);
                sum[2] += dataPtr2[2] * (w11 + w21);
                dataPtr2 += widthStep;
                sum[0] += dataPtr2[0] * w31;
                sum[1] += dataPtr2[1] * w31;
                sum[2] += dataPtr2[2] * w31;
                dataPtr2 += nChan2;
                sum[0] += 2 * dataPtr2[0] * (w32 + w33);
                sum[1] += 2 * dataPtr2[1] * (w32 + w33);
                sum[2] += 2 * dataPtr2[2] * (w32 + w33);

                sum[0] = Convert.ToInt32(sum[0] / wmain);
                sum[1] = Convert.ToInt32(sum[1] / wmain);
                sum[2] = Convert.ToInt32(sum[2] / wmain);
                if (sum[0] > 255)
                    sum[0] = 255;
                if (sum[1] > 255)
                    sum[1] = 255;
                if (sum[2] > 255)
                    sum[2] = 255;
                if (sum[0] < 0)
                    sum[0] = 0;
                if (sum[1] < 0)
                    sum[1] = 0;
                if (sum[2] < 0)
                    sum[2] = 0;
                dataPtr[0] = Convert.ToByte(sum[0]);
                dataPtr[1] = Convert.ToByte(sum[1]);
                dataPtr[2] = Convert.ToByte(sum[2]);

                dataPtr += widthStep;
                sum[0] = 0;
                sum[1] = 0;
                sum[2] = 0;

                //right
                dataPtr2 -= widthStep;
                for (int i = 0; i < height - 2; i++)
                {
                    sum[0] += dataPtr2[0] * (w12 + w13);
                    sum[1] += dataPtr2[1] * (w12 + w13);
                    sum[2] += dataPtr2[2] * (w12 + w13);
                    dataPtr2 -= nChan2;
                    sum[0] += dataPtr2[0] * w11;
                    sum[1] += dataPtr2[1] * w11;
                    sum[2] += dataPtr2[2] * w11;
                    dataPtr2 += widthStep;
                    sum[0] += dataPtr2[0] * w21;
                    sum[1] += dataPtr2[1] * w21;
                    sum[2] += dataPtr2[2] * w21;
                    dataPtr2 += widthStep;
                    sum[0] += dataPtr2[0] * w31;
                    sum[1] += dataPtr2[1] * w31;
                    sum[2] += dataPtr2[2] * w31;
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * (w32 + w33);
                    sum[1] += dataPtr2[1] * (w32 + w33);
                    sum[2] += dataPtr2[2] * (w32 + w33);
                    dataPtr2 -= widthStep;
                    sum[0] += dataPtr2[0] * (w22 + w23);
                    sum[1] += dataPtr2[1] * (w22 + w23);
                    sum[2] += dataPtr2[2] * (w22 + w23);

                    sum[0] = Convert.ToInt32(sum[0] / wmain);
                    sum[1] = Convert.ToInt32(sum[1] / wmain);
                    sum[2] = Convert.ToInt32(sum[2] / wmain);
                    if (sum[0] > 255)
                        sum[0] = 255;
                    if (sum[1] > 255)
                        sum[1] = 255;
                    if (sum[2] > 255)
                        sum[2] = 255;
                    if (sum[0] < 0)
                        sum[0] = 0;
                    if (sum[1] < 0)
                        sum[1] = 0;
                    if (sum[2] < 0)
                        sum[2] = 0;
                    dataPtr[0] = Convert.ToByte(sum[0]);
                    dataPtr[1] = Convert.ToByte(sum[1]);
                    dataPtr[2] = Convert.ToByte(sum[2]);

                    dataPtr += widthStep;

                    sum[0] = 0;
                    sum[1] = 0;
                    sum[2] = 0;
                }

                //bottom corner right

                dataPtr2 += widthStep;

                sum[0] += dataPtr2[0] * (w32 + w33 + w22 + w23);
                sum[1] += dataPtr2[1] * (w32 + w33 + w22 + w23);
                sum[2] += dataPtr2[2] * (w32 + w33 + w22 + w23);
                dataPtr2 -= nChan2;
                sum[0] += dataPtr2[0] * (w31 + w21);
                sum[1] += dataPtr2[2] * (w31 + w21);
                sum[2] += dataPtr2[2] * (w31 + w21);
                dataPtr2 -= widthStep;
                sum[0] += dataPtr2[0] * w11;
                sum[1] += dataPtr2[1] * w11;
                sum[2] += dataPtr2[2] * w11;
                dataPtr2 += nChan2;
                sum[0] += dataPtr2[0] * (w12 + w13);
                sum[1] += dataPtr2[1] * (w12 + w13);
                sum[2] += dataPtr2[2] * (w12 + w13);

                sum[0] = Convert.ToInt32(sum[0] / wmain);
                sum[1] = Convert.ToInt32(sum[1] / wmain);
                sum[2] = Convert.ToInt32(sum[2] / wmain);
                if (sum[0] > 255)
                    sum[0] = 255;
                if (sum[1] > 255)
                    sum[1] = 255;
                if (sum[2] > 255)
                    sum[2] = 255;
                if (sum[0] < 0)
                    sum[0] = 0;
                if (sum[1] < 0)
                    sum[1] = 0;
                if (sum[2] < 0)
                    sum[2] = 0;
                dataPtr[0] = Convert.ToByte(sum[0]);
                dataPtr[1] = Convert.ToByte(sum[1]);
                dataPtr[2] = Convert.ToByte(sum[2]);

                sum[0] = 0;
                sum[1] = 0;
                sum[2] = 0;

                dataPtr -= nChan;
                dataPtr2 += widthStep;
                //bottom 
                for (int i = 0; i < width - 2; i++)
                {
                    sum[0] += dataPtr2[0] * (w33 + w23);
                    sum[1] += dataPtr2[1] * (w33 + w23);
                    sum[2] += dataPtr2[2] * (w33 + w23);
                    dataPtr2 -= nChan2;
                    sum[0] += dataPtr2[0] * (w32 + w22);
                    sum[1] += dataPtr2[1] * (w32 + w22);
                    sum[2] += dataPtr2[2] * (w32 + w22);
                    dataPtr2 -= nChan2;
                    sum[0] += dataPtr2[0] * (w31 + w21);
                    sum[1] += dataPtr2[1] * (w31 + w21);
                    sum[2] += dataPtr2[2] * (w31 + w21);
                    dataPtr2 -= widthStep;
                    sum[0] += dataPtr2[0] * w11;
                    sum[1] += dataPtr2[1] * w11;
                    sum[2] += dataPtr2[2] * w11;
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * w12;
                    sum[1] += dataPtr2[1] * w12;
                    sum[2] += dataPtr2[2] * w12;
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * w13;
                    sum[1] += dataPtr2[1] * w13;
                    sum[2] += dataPtr2[2] * w13;

                    sum[0] = Convert.ToInt32(sum[0] / wmain);
                    sum[1] = Convert.ToInt32(sum[1] / wmain);
                    sum[2] = Convert.ToInt32(sum[2] / wmain);
                    if (sum[0] > 255)
                        sum[0] = 255;
                    if (sum[1] > 255)
                        sum[1] = 255;
                    if (sum[2] > 255)
                        sum[2] = 255;
                    if (sum[0] < 0)
                        sum[0] = 0;
                    if (sum[1] < 0)
                        sum[1] = 0;
                    if (sum[2] < 0)
                        sum[2] = 0;
                    dataPtr[0] = Convert.ToByte(sum[0]);
                    dataPtr[1] = Convert.ToByte(sum[1]);
                    dataPtr[2] = Convert.ToByte(sum[2]);

                    sum[0] = 0;
                    sum[1] = 0;
                    sum[2] = 0;

                    dataPtr -= nChan;
                    dataPtr2 += widthStep - nChan2;
                }
                /////////////////////
                //left corner bottom
                dataPtr += nChan;
                dataPtr2 += -nChan2;

                sum[0] += dataPtr2[0] * (w21 + w22 + w31 + w32);
                sum[1] += dataPtr2[1] * (w21 + w22 + w31 + w32);
                sum[2] += dataPtr2[2] * (w21 + w22 + w31 + w32);
                dataPtr2 += nChan2;
                sum[0] += dataPtr2[0] * (w23 + w33);
                sum[1] += dataPtr2[2] * (w23 + w33);
                sum[2] += dataPtr2[2] * (w23 + w33);
                dataPtr2 -= widthStep;
                sum[0] += dataPtr2[0] * w13;
                sum[1] += dataPtr2[1] * w13;
                sum[2] += dataPtr2[2] * w13;
                dataPtr2 -= nChan2;
                sum[0] += dataPtr2[0] * (w11 + w12);
                sum[1] += dataPtr2[1] * (w11 + w12);
                sum[2] += dataPtr2[2] * (w11 + w12);

                sum[0] = Convert.ToInt32(sum[0] / wmain);
                sum[1] = Convert.ToInt32(sum[1] / wmain);
                sum[2] = Convert.ToInt32(sum[2] / wmain);
                if (sum[0] > 255)
                    sum[0] = 255;
                if (sum[1] > 255)
                    sum[1] = 255;
                if (sum[2] > 255)
                    sum[2] = 255;
                if (sum[0] < 0)
                    sum[0] = 0;
                if (sum[1] < 0)
                    sum[1] = 0;
                if (sum[2] < 0)
                    sum[2] = 0;
                dataPtr[0] = Convert.ToByte(sum[0]);
                dataPtr[1] = Convert.ToByte(sum[1]);
                dataPtr[2] = Convert.ToByte(sum[2]);

                sum[0] = 0;
                sum[1] = 0;
                sum[2] = 0;

                dataPtr -= widthStep;
                dataPtr2 += widthStep;
                //left 
                for (int i = 0; i < height - 2; i++)
                {
                    sum[0] += dataPtr2[0] * (w31 + w32);
                    sum[1] += dataPtr2[1] * (w31 + w32);
                    sum[2] += dataPtr2[2] * (w31 + w32);
                    dataPtr2 -= widthStep;
                    sum[0] += dataPtr2[0] * (w21 + w22);
                    sum[1] += dataPtr2[1] * (w21 + w22);
                    sum[2] += dataPtr2[2] * (w21 + w22);
                    dataPtr2 -= widthStep;
                    sum[0] += dataPtr2[0] * (w11 + w12);
                    sum[1] += dataPtr2[1] * (w11 + w12);
                    sum[2] += dataPtr2[2] * (w11 + w12);
                    dataPtr2 += nChan2;
                    sum[0] += dataPtr2[0] * w13;
                    sum[1] += dataPtr2[1] * w13;
                    sum[2] += dataPtr2[2] * w13;
                    dataPtr2 += widthStep;
                    sum[0] += dataPtr2[0] * w23;
                    sum[1] += dataPtr2[1] * w23;
                    sum[2] += dataPtr2[2] * w23;
                    dataPtr2 += widthStep;
                    sum[0] += dataPtr2[0] * w33;
                    sum[1] += dataPtr2[1] * w33;
                    sum[2] += dataPtr2[2] * w33;

                    sum[0] = Convert.ToInt32(sum[0] / wmain);
                    sum[1] = Convert.ToInt32(sum[1] / wmain);
                    sum[2] = Convert.ToInt32(sum[2] / wmain);
                    if (sum[0] > 255)
                        sum[0] = 255;
                    if (sum[1] > 255)
                        sum[1] = 255;
                    if (sum[2] > 255)
                        sum[2] = 255;
                    if (sum[0] < 0)
                        sum[0] = 0;
                    if (sum[1] < 0)
                        sum[1] = 0;
                    if (sum[2] < 0)
                        sum[2] = 0;
                    dataPtr[0] = Convert.ToByte(sum[0]);
                    dataPtr[1] = Convert.ToByte(sum[1]);
                    dataPtr[2] = Convert.ToByte(sum[2]);

                    sum[0] = 0;
                    sum[1] = 0;
                    sum[2] = 0;

                    dataPtr -= widthStep;
                    dataPtr2 += -nChan2 - widthStep;
                }

                dataPtr = dataPtrpom;
                dataPtr2 = dataPtrpom2;
                
            }
        }

        internal static void Roberts(Image<Bgr, byte> img)
        {
            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int widthStep = m.widthStep;
                int widthStep2 = m2.widthStep;
                int width2 = imgcopy.Width;
                int height2 = imgcopy.Height;

                byte* dataPtrpom = dataPtr;
                byte* dataPtrpom2 = dataPtr2;

                int[] pixels = new int[3];

                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;

                //centre
                dataPtr += widthStep;
                dataPtr2 += widthStep2;

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        pixels[0] += Math.Abs(dataPtr2[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 + nChan2)[0]);                        
                        pixels[1] += Math.Abs(dataPtr2[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 + nChan2)[1]);
                        pixels[2] += Math.Abs(dataPtr2[2] - (dataPtr2 + widthStep2 + nChan2)[2]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 + nChan2)[2]);
                       
                        if (pixels[0] > 255)
                            pixels[0] = 255;
                        if (pixels[1] > 255)
                            pixels[1] = 255;
                        if (pixels[2] > 255)
                            pixels[2] = 255;
                        if (pixels[0] < 0)
                            pixels[0] = 0;
                        if (pixels[1] < 0)
                            pixels[1] = 0;
                        if (pixels[2] < 0)
                            pixels[2] = 0;
                        dataPtr[0] = Convert.ToByte(pixels[0]);
                        dataPtr[1] = Convert.ToByte(pixels[1]);
                        dataPtr[2] = Convert.ToByte(pixels[2]);
                        pixels[0] = 0;
                        pixels[1] = 0;
                        pixels[2] = 0;
                        dataPtr += nChan;
                        dataPtr2 += nChan2;
                    }
                    dataPtr += nChan + padding;
                    dataPtr2 += nChan2 + padding2;
                }

                //top
                dataPtr = dataPtrpom;
                dataPtr2 = dataPtrpom2;

                for (int x = 0; x < width2 - 1; x++)
                {
                    pixels[0] += Math.Abs(dataPtr2[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 + nChan2)[0]);                    
                    pixels[1] += Math.Abs(dataPtr2[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 + nChan2)[1]);
                    pixels[2] += Math.Abs(dataPtr2[2] - (dataPtr2 + widthStep2 + nChan2)[2]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 + nChan2)[2]);
                    
                    if (pixels[0] > 255)
                        pixels[0] = 255;
                    if (pixels[1] > 255)
                        pixels[1] = 255;
                    if (pixels[2] > 255)
                        pixels[2] = 255;
                    if (pixels[0] < 0)
                        pixels[0] = 0;
                    if (pixels[1] < 0)
                        pixels[1] = 0;
                    if (pixels[2] < 0)
                        pixels[2] = 0;
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;

                    dataPtr += nChan;
                    dataPtr2 += nChan2;
                }
                //right
                for (int y = 0; y < height2 - 1; y++)
                {
                    pixels[0] += Math.Abs(dataPtr2[0] - (dataPtr2 + widthStep2)[0] + Math.Abs((dataPtr2 + widthStep2)[0] - dataPtr2[0]));
                    pixels[1] += Math.Abs(dataPtr2[1] - (dataPtr2 + widthStep2)[1] + Math.Abs((dataPtr2 + widthStep2)[1] - dataPtr2[1]));
                    pixels[2] += Math.Abs(dataPtr2[2] - (dataPtr2 + widthStep2)[2] + Math.Abs((dataPtr2 + widthStep2)[2] - dataPtr2[2]));
                    
                    if (pixels[0] > 255)
                        pixels[0] = 255;
                    if (pixels[1] > 255)
                        pixels[1] = 255;
                    if (pixels[2] > 255)
                        pixels[2] = 255;
                    if (pixels[0] < 0)
                        pixels[0] = 0;
                    if (pixels[1] < 0)
                        pixels[1] = 0;
                    if (pixels[2] < 0)
                        pixels[2] = 0;
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;

                    dataPtr += widthStep;
                    dataPtr2 += widthStep2;
                }
                //bottom right corner
                dataPtr[0] = 0;
                dataPtr[1] = 0;
                dataPtr[2] = 0;
                //bottom
                dataPtr -= nChan;
                dataPtr2 -= nChan2;
                for(int x = 0; x < width2 - 1; x++)
                {
                    pixels[0] += Math.Abs(dataPtr2[0] - (dataPtr2 + nChan2)[0])*2;
                    pixels[1] += Math.Abs(dataPtr2[1] - (dataPtr2 + nChan2)[1])*2;
                    pixels[2] += Math.Abs(dataPtr2[2] - (dataPtr2 + nChan2)[2])*2;

                    if (pixels[0] > 255)
                        pixels[0] = 255;
                    if (pixels[1] > 255)
                        pixels[1] = 255;
                    if (pixels[2] > 255)
                        pixels[2] = 255;
                    if (pixels[0] < 0)
                        pixels[0] = 0;
                    if (pixels[1] < 0)
                        pixels[1] = 0;
                    if (pixels[2] < 0)
                        pixels[2] = 0;
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;

                    dataPtr -= nChan;
                    dataPtr2 -= nChan2;
                }
                //left bottom corner
                pixels[0] += Math.Abs(dataPtr2[0] - (dataPtr2 + nChan2)[0]) * 2;
                pixels[1] += Math.Abs(dataPtr2[1] - (dataPtr2 + nChan2)[1]) * 2;
                pixels[2] += Math.Abs(dataPtr2[2] - (dataPtr2 + nChan2)[2]) * 2;

                if (pixels[0] > 255)
                    pixels[0] = 255;
                if (pixels[1] > 255)
                    pixels[1] = 255;
                if (pixels[2] > 255)
                    pixels[2] = 255;
                if (pixels[0] < 0)
                    pixels[0] = 0;
                if (pixels[1] < 0)
                    pixels[1] = 0;
                if (pixels[2] < 0)
                    pixels[2] = 0;
                dataPtr[0] = Convert.ToByte(pixels[0]);
                dataPtr[1] = Convert.ToByte(pixels[1]);
                dataPtr[2] = Convert.ToByte(pixels[2]);
            }
        }

        internal static void Sobel(Image<Bgr, byte> img)
        {
            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int widthStep = m.widthStep;
                int widthStep2 = m2.widthStep;
                int width2 = imgcopy.Width;
                int height2 = imgcopy.Height;

                byte* dataPtrpom = dataPtr;
                byte* dataPtrpom2 = dataPtr2;
                int[] pixels1 = new int[3];
                int[] pixels2 = new int[3];
                int[] s = new int[3];


                pixels1[0] = 0;
                pixels1[1] = 0;
                pixels1[2] = 0;

                pixels2[0] = 0;
                pixels2[1] = 0;
                pixels2[2] = 0;

                s[0] = 0;
                s[1] = 0;
                s[2] = 0;

                dataPtr += widthStep + nChan;
                dataPtr2 += widthStep + nChan2;

                //centre
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        //left pixels1
                        pixels1[0] += (dataPtr2 - widthStep2 - nChan2)[0];
                        pixels1[1] += (dataPtr2 - widthStep2 - nChan2)[1];
                        pixels1[2] += (dataPtr2 - widthStep2 - nChan2)[2];
                        pixels1[0] += 2 * (dataPtr2 - nChan2)[0];
                        pixels1[1] += 2 * (dataPtr2 - nChan2)[1];
                        pixels1[2] += 2 * (dataPtr2 - nChan2)[2];
                        pixels1[0] += (dataPtr2 + widthStep - nChan2)[0];
                        pixels1[1] += (dataPtr2 + widthStep - nChan2)[1];
                        pixels1[2] += (dataPtr2 + widthStep - nChan2)[2];
                        //right pixels1
                        pixels1[0] = pixels1[0] - (dataPtr2 - widthStep2 + nChan2)[0];
                        pixels1[1] = pixels1[1] - (dataPtr2 - widthStep2 + nChan2)[1];
                        pixels1[2] = pixels1[2] - (dataPtr2 - widthStep2 + nChan2)[2];
                        pixels1[0] = pixels1[0] - 2 * (dataPtr2 + nChan2)[0];
                        pixels1[1] = pixels1[1] - 2 * (dataPtr2 + nChan2)[1];
                        pixels1[2] = pixels1[2] - 2 * (dataPtr2 + nChan2)[2];
                        pixels1[0] = pixels1[0] - (dataPtr2 + widthStep + nChan2)[0];
                        pixels1[1] = pixels1[1] - (dataPtr2 + widthStep + nChan2)[1];
                        pixels1[2] = pixels1[2] - (dataPtr2 + widthStep + nChan2)[2];

                        //bottom pixels2
                        pixels2[0] += (dataPtr2 + widthStep - nChan)[0];
                        pixels2[1] += (dataPtr2 + widthStep - nChan)[1];
                        pixels2[2] += (dataPtr2 + widthStep - nChan)[2];
                        pixels2[0] += 2 * (dataPtr2 + widthStep)[0];
                        pixels2[1] += 2 * (dataPtr2 + widthStep)[1];
                        pixels2[2] += 2 * (dataPtr2 + widthStep)[2];
                        pixels2[0] += (dataPtr2 + widthStep + nChan)[0];
                        pixels2[1] += (dataPtr2 + widthStep + nChan)[1];
                        pixels2[2] += (dataPtr2 + widthStep + nChan)[2];
                        //top pixels2
                        pixels2[0] = pixels2[0] - (dataPtr2 - widthStep - nChan)[0];
                        pixels2[1] = pixels2[1] - (dataPtr2 - widthStep - nChan)[1];
                        pixels2[2] = pixels2[2] - (dataPtr2 - widthStep - nChan)[2];
                        pixels2[0] = pixels2[0] - 2 * (dataPtr2 - widthStep)[0];
                        pixels2[1] = pixels2[1] - 2 * (dataPtr2 - widthStep)[1];
                        pixels2[2] = pixels2[2] - 2 * (dataPtr2 - widthStep)[2];
                        pixels2[0] = pixels2[0] - (dataPtr2 - widthStep + nChan)[0];
                        pixels2[1] = pixels2[1] - (dataPtr2 - widthStep + nChan)[1];
                        pixels2[2] = pixels2[2] - (dataPtr2 - widthStep + nChan)[2];


                        s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                        s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                        s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);


                        //check
                        if (s[0] > 255)
                            s[0] = 255;
                        if (s[1] > 255)
                            s[1] = 255;
                        if (s[2] > 255)
                            s[2] = 255;
                        if (s[0] < 0)
                            s[0] = 0;
                        if (s[1] < 0)
                            s[1] = 0;
                        if (s[2] < 0)
                            s[2] = 0;

                        dataPtr[0] = Convert.ToByte(s[0]);
                        dataPtr[1] = Convert.ToByte(s[1]);
                        dataPtr[2] = Convert.ToByte(s[2]);


                        s[0] = 0;
                        s[1] = 0;
                        s[2] = 0;

                        pixels1[0] = 0;
                        pixels1[1] = 0;
                        pixels1[2] = 0;

                        pixels2[0] = 0;
                        pixels2[1] = 0;
                        pixels2[2] = 0;

                        dataPtr += nChan;
                        dataPtr2 += nChan2;


                    }
                    dataPtr += 2 * nChan + padding;
                    dataPtr2 += 2 * nChan2 + padding2;
                }

                dataPtr = dataPtrpom;
                dataPtr2 = dataPtrpom2;

                //left corner
                //left pixels1
                pixels1[0] += 3 * dataPtr2[0];
                pixels1[1] += 3 * dataPtr2[1];
                pixels1[2] += 3 * dataPtr2[2];
                pixels1[0] += (dataPtr2 + widthStep)[0];
                pixels1[1] += (dataPtr2 + widthStep)[1];
                pixels1[2] += (dataPtr2 + widthStep)[2];

                //right pixels1
                pixels1[0] = pixels1[0] - 3 * (dataPtr2 + nChan2)[0];
                pixels1[1] = pixels1[1] - 3 * (dataPtr2 + nChan2)[1];
                pixels1[2] = pixels1[2] - 3 * (dataPtr2 + nChan2)[2];
                pixels1[0] = pixels1[0] - (dataPtr2 + widthStep + nChan2)[0];
                pixels1[1] = pixels1[1] - (dataPtr2 + widthStep + nChan2)[1];
                pixels1[2] = pixels1[2] - (dataPtr2 + widthStep + nChan2)[2];

                //bottom pixels2
                pixels2[0] += 3 * (dataPtr2 + widthStep)[0];
                pixels2[1] += 3 * (dataPtr2 + widthStep)[1];
                pixels2[2] += 3 * (dataPtr2 + widthStep)[2];
                pixels2[0] += (dataPtr2 + widthStep + nChan)[0];
                pixels2[1] += (dataPtr2 + widthStep + nChan)[1];
                pixels2[2] += (dataPtr2 + widthStep + nChan)[2];
                //top pixels2
                pixels2[0] = pixels2[0] - 3 * (dataPtr2)[0];
                pixels2[1] = pixels2[1] - 3 * (dataPtr2)[1];
                pixels2[2] = pixels2[2] - 3 * (dataPtr2)[2];
                pixels2[0] = pixels2[0] - (dataPtr2 + nChan)[0];
                pixels2[1] = pixels2[1] - (dataPtr2 + nChan)[1];
                pixels2[2] = pixels2[2] - (dataPtr2 + nChan)[2];


                s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);


                //check
                if (s[0] > 255)
                    s[0] = 255;
                if (s[1] > 255)
                    s[1] = 255;
                if (s[2] > 255)
                    s[2] = 255;
                if (s[0] < 0)
                    s[0] = 0;
                if (s[1] < 0)
                    s[1] = 0;
                if (s[2] < 0)
                    s[2] = 0;

                dataPtr[0] = Convert.ToByte(s[0]);
                dataPtr[1] = Convert.ToByte(s[1]);
                dataPtr[2] = Convert.ToByte(s[2]);

                s[0] = 0;
                s[1] = 0;
                s[2] = 0;

                pixels1[0] = 0;
                pixels1[1] = 0;
                pixels1[2] = 0;

                pixels2[0] = 0;
                pixels2[1] = 0;
                pixels2[2] = 0;

                //frame top
                dataPtr = dataPtrpom + nChan;
                dataPtr2 = dataPtrpom2 + nChan;

                for (int i = 0; i < width - 2; i++)
                {
                    //left pixels1
                    pixels1[0] += 3 * (dataPtr2 - nChan2)[0];
                    pixels1[1] += 3 * (dataPtr2 - nChan2)[1];
                    pixels1[2] += 3 * (dataPtr2 - nChan2)[2];
                    pixels1[0] += (dataPtr2 + widthStep - nChan2)[0];
                    pixels1[1] += (dataPtr2 + widthStep - nChan2)[1];
                    pixels1[2] += (dataPtr2 + widthStep - nChan2)[2];

                    //right pixels1
                    pixels1[0] = pixels1[0] - 3 * (dataPtr2 + nChan2)[0];
                    pixels1[1] = pixels1[1] - 3 * (dataPtr2 + nChan2)[1];
                    pixels1[2] = pixels1[2] - 3 * (dataPtr2 + nChan2)[2];
                    pixels1[0] = pixels1[0] - (dataPtr2 + widthStep + nChan2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 + widthStep + nChan2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 + widthStep + nChan2)[2];

                    //bottom pixels2
                    pixels2[0] += (dataPtr2 + widthStep - nChan)[0];
                    pixels2[1] += (dataPtr2 + widthStep - nChan)[1];
                    pixels2[2] += (dataPtr2 + widthStep - nChan)[2];
                    pixels2[0] += 2 * (dataPtr2 + widthStep)[0];
                    pixels2[1] += 2 * (dataPtr2 + widthStep)[1];
                    pixels2[2] += 2 * (dataPtr2 + widthStep)[2];
                    pixels2[0] += (dataPtr2 + widthStep + nChan)[0];
                    pixels2[1] += (dataPtr2 + widthStep + nChan)[1];
                    pixels2[2] += (dataPtr2 + widthStep + nChan)[2];
                    //top pixels2
                    pixels2[0] = pixels2[0] - (dataPtr2 - nChan)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 - nChan)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 - nChan)[2];
                    pixels2[0] = pixels2[0] - 2 * (dataPtr2)[0];
                    pixels2[1] = pixels2[1] - 2 * (dataPtr2)[1];
                    pixels2[2] = pixels2[2] - 2 * (dataPtr2)[2];
                    pixels2[0] = pixels2[0] - (dataPtr2 + nChan)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 + nChan)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 + nChan)[2];

                    s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                    s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                    s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);


                    //check
                    if (s[0] > 255)
                        s[0] = 255;
                    if (s[1] > 255)
                        s[1] = 255;
                    if (s[2] > 255)
                        s[2] = 255;
                    if (s[0] < 0)
                        s[0] = 0;
                    if (s[1] < 0)
                        s[1] = 0;
                    if (s[2] < 0)
                        s[2] = 0;

                    dataPtr[0] = Convert.ToByte(s[0]);
                    dataPtr[1] = Convert.ToByte(s[1]);
                    dataPtr[2] = Convert.ToByte(s[2]);


                    s[0] = 0;
                    s[1] = 0;
                    s[2] = 0;

                    pixels1[0] = 0;
                    pixels1[1] = 0;
                    pixels1[2] = 0;

                    pixels2[0] = 0;
                    pixels2[1] = 0;
                    pixels2[2] = 0;

                    dataPtr += nChan;
                    dataPtr2 += nChan2;

                }
                //right corner 



                //left pixels1
                pixels1[0] += 3 * (dataPtr2 - nChan2)[0];
                pixels1[1] += 3 * (dataPtr2 - nChan2)[1];
                pixels1[2] += 3 * (dataPtr2 - nChan2)[2];
                pixels1[0] += (dataPtr2 + widthStep2 - nChan2)[0];
                pixels1[1] += (dataPtr2 + widthStep2 - nChan2)[1];
                pixels1[2] += (dataPtr2 + widthStep2 - nChan2)[2];
                //right pixels1
                pixels1[0] = pixels1[0] - 3 * (dataPtr2)[0];
                pixels1[1] = pixels1[1] - 3 * (dataPtr2)[1];
                pixels1[2] = pixels1[2] - 3 * (dataPtr2)[2];
                pixels1[0] = pixels1[0] - (dataPtr2 + widthStep)[0];
                pixels1[1] = pixels1[1] - (dataPtr2 + widthStep)[1];
                pixels1[2] = pixels1[2] - (dataPtr2 + widthStep)[2];

                //bottom pixels2
                pixels2[0] += (dataPtr2 + widthStep - nChan2)[0];
                pixels2[1] += (dataPtr2 + widthStep - nChan2)[1];
                pixels2[2] += (dataPtr2 + widthStep - nChan2)[2];
                pixels2[0] += 3 * (dataPtr2 + widthStep)[0];
                pixels2[1] += 3 * (dataPtr2 + widthStep)[1];
                pixels2[2] += 3 * (dataPtr2 + widthStep)[2];
                //top pixels2
                pixels2[0] = pixels2[0] - (dataPtr2 - nChan2)[0];
                pixels2[1] = pixels2[1] - (dataPtr2 - nChan2)[1];
                pixels2[2] = pixels2[2] - (dataPtr2 - nChan2)[2];
                pixels2[0] = pixels2[0] - 3 * (dataPtr2)[0];
                pixels2[1] = pixels2[1] - 3 * (dataPtr2)[1];
                pixels2[2] = pixels2[2] - 3 * (dataPtr2)[2];


                s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);


                //check
                if (s[0] > 255)
                    s[0] = 255;
                if (s[1] > 255)
                    s[1] = 255;
                if (s[2] > 255)
                    s[2] = 255;
                if (s[0] < 0)
                    s[0] = 0;
                if (s[1] < 0)
                    s[1] = 0;
                if (s[2] < 0)
                    s[2] = 0;



                dataPtr[0] = Convert.ToByte(s[0]);
                dataPtr[1] = Convert.ToByte(s[1]);
                dataPtr[2] = Convert.ToByte(s[2]);


                s[0] = 0;
                s[1] = 0;
                s[2] = 0;

                pixels1[0] = 0;
                pixels1[1] = 0;
                pixels1[2] = 0;


                //right frame

                dataPtr += widthStep;
                dataPtr2 += widthStep;

                for (int i = 0; i < height - 2; i++)
                {

                    //left pixels1
                    pixels1[0] += (dataPtr2 - nChan2 - widthStep2)[0];
                    pixels1[1] += (dataPtr2 - nChan2 - widthStep2)[1];
                    pixels1[2] += (dataPtr2 - nChan2 - widthStep2)[2];
                    pixels1[0] += 2 * (dataPtr2 - nChan2)[0];
                    pixels1[1] += 2 * (dataPtr2 - nChan2)[1];
                    pixels1[2] += 2 * (dataPtr2 - nChan2)[2];
                    pixels1[0] += (dataPtr2 + widthStep2 - nChan2)[0];
                    pixels1[1] += (dataPtr2 + widthStep2 - nChan2)[1];
                    pixels1[2] += (dataPtr2 + widthStep2 - nChan2)[2];
                    //right pixels1
                    pixels1[0] = pixels1[0] - (dataPtr2 - widthStep2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 - widthStep2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 - widthStep2)[2];
                    pixels1[0] = pixels1[0] - 2 * (dataPtr2)[0];
                    pixels1[1] = pixels1[1] - 2 * (dataPtr2)[1];
                    pixels1[2] = pixels1[2] - 2 * (dataPtr2)[2];
                    pixels1[0] = pixels1[0] - (dataPtr2 + widthStep2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 + widthStep2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 + widthStep2)[2];

                    //bottom pixels2
                    pixels2[0] += (dataPtr2 + widthStep - nChan2)[0];
                    pixels2[1] += (dataPtr2 + widthStep - nChan2)[1];
                    pixels2[2] += (dataPtr2 + widthStep - nChan2)[2];
                    pixels2[0] += 3 * (dataPtr2 + widthStep)[0];
                    pixels2[1] += 3 * (dataPtr2 + widthStep)[1];
                    pixels2[2] += 3 * (dataPtr2 + widthStep)[2];
                    //top pixels2
                    pixels2[0] = pixels2[0] - (dataPtr2 - nChan2 - widthStep)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 - nChan2 - widthStep)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 - nChan2 - widthStep)[2];
                    pixels2[0] = pixels2[0] - 3 * (dataPtr2 - widthStep)[0];
                    pixels2[1] = pixels2[1] - 3 * (dataPtr2 - widthStep)[1];
                    pixels2[2] = pixels2[2] - 3 * (dataPtr2 - widthStep)[2];


                    s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                    s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                    s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);

                    //check
                    if (s[0] > 255)
                        s[0] = 255;
                    if (s[1] > 255)
                        s[1] = 255;
                    if (s[2] > 255)
                        s[2] = 255;
                    if (s[0] < 0)
                        s[0] = 0;
                    if (s[1] < 0)
                        s[1] = 0;
                    if (s[2] < 0)
                        s[2] = 0;



                    dataPtr[0] = Convert.ToByte(s[0]);
                    dataPtr[1] = Convert.ToByte(s[1]);
                    dataPtr[2] = Convert.ToByte(s[2]);

                    s[0] = 0;
                    s[1] = 0;
                    s[2] = 0;

                    pixels1[0] = 0;
                    pixels1[1] = 0;
                    pixels1[2] = 0;

                    pixels2[0] = 0;
                    pixels2[1] = 0;
                    pixels2[2] = 0;

                    dataPtr += widthStep;
                    dataPtr2 += widthStep2;


                }


                //right bottom corrner

                //left pixels1
                pixels1[0] += (dataPtr2 - nChan2 - widthStep2)[0];
                pixels1[1] += (dataPtr2 - nChan2 - widthStep2)[1];
                pixels1[2] += (dataPtr2 - nChan2 - widthStep2)[2];
                pixels1[0] += 3 * (dataPtr2)[0];
                pixels1[1] += 3 * (dataPtr2)[1];
                pixels1[2] += 3 * (dataPtr2)[2];
                //right pixels1
                pixels1[0] = pixels1[0] - (dataPtr2 - widthStep)[0];
                pixels1[1] = pixels1[1] - (dataPtr2 - widthStep)[1];
                pixels1[2] = pixels1[2] - (dataPtr2 - widthStep)[2];
                pixels1[0] = pixels1[0] - 3 * (dataPtr2)[0];
                pixels1[1] = pixels1[1] - 3 * (dataPtr2)[1];
                pixels1[2] = pixels1[2] - 3 * (dataPtr2)[2];
                //bottom pixels2
                pixels2[0] += (dataPtr2 - nChan2)[0];
                pixels2[1] += (dataPtr2 - nChan2)[1];
                pixels2[2] += (dataPtr2 - nChan2)[2];
                pixels2[0] += 3 * (dataPtr2)[0];
                pixels2[1] += 3 * (dataPtr2)[1];
                pixels2[2] += 3 * (dataPtr2)[2];
                //top pixels2
                pixels2[0] = pixels2[0] - (dataPtr2 - nChan2 - widthStep)[0];
                pixels2[1] = pixels2[1] - (dataPtr2 - nChan2 - widthStep)[1];
                pixels2[2] = pixels2[2] - (dataPtr2 - nChan2 - widthStep)[2];
                pixels2[0] = pixels2[0] - 3 * (dataPtr2 - widthStep)[0];
                pixels2[1] = pixels2[1] - 3 * (dataPtr2 - widthStep)[1];
                pixels2[2] = pixels2[2] - 3 * (dataPtr2 - widthStep)[2];


                s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);

                //check
                if (s[0] > 255)
                    s[0] = 255;
                if (s[1] > 255)
                    s[1] = 255;
                if (s[2] > 255)
                    s[2] = 255;
                if (s[0] < 0)
                    s[0] = 0;
                if (s[1] < 0)
                    s[1] = 0;
                if (s[2] < 0)
                    s[2] = 0;

                dataPtr[0] = Convert.ToByte(s[0]);
                dataPtr[1] = Convert.ToByte(s[1]);
                dataPtr[2] = Convert.ToByte(s[2]);

                s[0] = 0;
                s[1] = 0;
                s[2] = 0;

                pixels1[0] = 0;
                pixels1[1] = 0;
                pixels1[2] = 0;

                pixels2[0] = 0;
                pixels2[1] = 0;
                pixels2[2] = 0;


                dataPtr -= nChan;
                dataPtr2 -= nChan2;

                //bottom frame

                for (int i = 0; i < width - 2; i++)
                {
                    //left pixels1
                    pixels1[0] += 3 * (dataPtr2 - nChan2)[0];
                    pixels1[1] += 3 * (dataPtr2 - nChan2)[1];
                    pixels1[2] += 3 * (dataPtr2 - nChan2)[2];
                    pixels1[0] += (dataPtr2 - widthStep - nChan2)[0];
                    pixels1[1] += (dataPtr2 - widthStep - nChan2)[1];
                    pixels1[2] += (dataPtr2 - widthStep - nChan2)[2];

                    //right pixels1
                    pixels1[0] = pixels1[0] - 3 * (dataPtr2 + nChan2)[0];
                    pixels1[1] = pixels1[1] - 3 * (dataPtr2 + nChan2)[1];
                    pixels1[2] = pixels1[2] - 3 * (dataPtr2 + nChan2)[2];
                    pixels1[0] = pixels1[0] - (dataPtr2 - widthStep + nChan2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 - widthStep + nChan2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 - widthStep + nChan2)[2];

                    //bottom pixels2
                    pixels2[0] += (dataPtr2 - nChan)[0];
                    pixels2[1] += (dataPtr2 - nChan)[1];
                    pixels2[2] += (dataPtr2 - nChan)[2];
                    pixels2[0] += 2 * (dataPtr2)[0];
                    pixels2[1] += 2 * (dataPtr2)[1];
                    pixels2[2] += 2 * (dataPtr2)[2];
                    pixels2[0] += (dataPtr2 + nChan)[0];
                    pixels2[1] += (dataPtr2 + nChan)[1];
                    pixels2[2] += (dataPtr2 + nChan)[2];
                    //top pixels2
                    pixels2[0] = pixels2[0] - (dataPtr2 - widthStep2 - nChan)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 - widthStep2 - nChan)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 - widthStep2 - nChan)[2];
                    pixels2[0] = pixels2[0] - 2 * (dataPtr2 - widthStep2)[0];
                    pixels2[1] = pixels2[1] - 2 * (dataPtr2 - widthStep2)[1];
                    pixels2[2] = pixels2[2] - 2 * (dataPtr2 - widthStep2)[2];
                    pixels2[0] = pixels2[0] - (dataPtr2 + nChan - widthStep2)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 + nChan - widthStep2)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 + nChan - widthStep2)[2];

                    s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                    s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                    s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);


                    //check
                    if (s[0] > 255)
                        s[0] = 255;
                    if (s[1] > 255)
                        s[1] = 255;
                    if (s[2] > 255)
                        s[2] = 255;
                    if (s[0] < 0)
                        s[0] = 0;
                    if (s[1] < 0)
                        s[1] = 0;
                    if (s[2] < 0)
                        s[2] = 0;

                    dataPtr[0] = Convert.ToByte(s[0]);
                    dataPtr[1] = Convert.ToByte(s[1]);
                    dataPtr[2] = Convert.ToByte(s[2]);

                    s[0] = 0;
                    s[1] = 0;
                    s[2] = 0;

                    pixels1[0] = 0;
                    pixels1[1] = 0;
                    pixels1[2] = 0;

                    pixels2[0] = 0;
                    pixels2[1] = 0;
                    pixels2[2] = 0;

                    dataPtr -= nChan;
                    dataPtr2 -= nChan2;

                }
                //left bottom corrner

                //left pixels1
                pixels1[0] += 3 * (dataPtr2)[0];
                pixels1[1] += 3 * (dataPtr2)[1];
                pixels1[2] += 3 * (dataPtr2)[2];
                pixels1[0] += (dataPtr2 - widthStep2)[0];
                pixels1[1] += (dataPtr2 - widthStep2)[1];
                pixels1[2] += (dataPtr2 - widthStep2)[2];
                //right pixels1
                pixels1[0] = pixels1[0] - (dataPtr2 - widthStep + nChan2)[0];
                pixels1[1] = pixels1[1] - (dataPtr2 - widthStep + nChan2)[1];
                pixels1[2] = pixels1[2] - (dataPtr2 - widthStep + nChan2)[2];
                pixels1[0] = pixels1[0] - 3 * (dataPtr2 + nChan2)[0];
                pixels1[1] = pixels1[1] - 3 * (dataPtr2 + nChan2)[1];
                pixels1[2] = pixels1[2] - 3 * (dataPtr2 + nChan2)[2];
                //bottom pixels2
                pixels2[0] += (dataPtr2 + nChan2)[0];
                pixels2[1] += (dataPtr2 + nChan2)[1];
                pixels2[2] += (dataPtr2 + nChan2)[2];
                pixels2[0] += 3 * (dataPtr2)[0];
                pixels2[1] += 3 * (dataPtr2)[1];
                pixels2[2] += 3 * (dataPtr2)[2];
                //top pixels2
                pixels2[0] = pixels2[0] - (dataPtr2 + nChan2 - widthStep)[0];
                pixels2[1] = pixels2[1] - (dataPtr2 + nChan2 - widthStep)[1];
                pixels2[2] = pixels2[2] - (dataPtr2 + nChan2 - widthStep)[2];
                pixels2[0] = pixels2[0] - 3 * (dataPtr2 - widthStep)[0];
                pixels2[1] = pixels2[1] - 3 * (dataPtr2 - widthStep)[1];
                pixels2[2] = pixels2[2] - 3 * (dataPtr2 - widthStep)[2];


                s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);

                //check
                if (s[0] > 255)
                    s[0] = 255;
                if (s[1] > 255)
                    s[1] = 255;
                if (s[2] > 255)
                    s[2] = 255;
                if (s[0] < 0)
                    s[0] = 0;
                if (s[1] < 0)
                    s[1] = 0;
                if (s[2] < 0)
                    s[2] = 0;

                dataPtr[0] = Convert.ToByte(s[0]);
                dataPtr[1] = Convert.ToByte(s[1]);
                dataPtr[2] = Convert.ToByte(s[2]);

                s[0] = 0;
                s[1] = 0;
                s[2] = 0;

                pixels1[0] = 0;
                pixels1[1] = 0;
                pixels1[2] = 0;

                pixels2[0] = 0;
                pixels2[1] = 0;
                pixels2[2] = 0;



                //left frame

                dataPtr -= widthStep;
                dataPtr2 -= widthStep;



                for (int i = 0; i < height - 2; i++)
                {

                    //left pixels1
                    pixels1[0] += (dataPtr2 - widthStep2)[0];
                    pixels1[1] += (dataPtr2 - widthStep2)[1];
                    pixels1[2] += (dataPtr2 - widthStep2)[2];
                    pixels1[0] += 2 * (dataPtr2)[0];
                    pixels1[1] += 2 * (dataPtr2)[1];
                    pixels1[2] += 2 * (dataPtr2)[2];
                    pixels1[0] += (dataPtr2 + widthStep2)[0];
                    pixels1[1] += (dataPtr2 + widthStep2)[1];
                    pixels1[2] += (dataPtr2 + widthStep2)[2];
                    //right pixels1
                    pixels1[0] = pixels1[0] - (dataPtr2 - widthStep2 + nChan2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 - widthStep2 + nChan2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 - widthStep2 + nChan2)[2];
                    pixels1[0] = pixels1[0] - 2 * (dataPtr2 + nChan2)[0];
                    pixels1[1] = pixels1[1] - 2 * (dataPtr2 + nChan2)[1];
                    pixels1[2] = pixels1[2] - 2 * (dataPtr2 + nChan2)[2];
                    pixels1[0] = pixels1[0] - (dataPtr2 + widthStep2 + nChan2)[0];
                    pixels1[1] = pixels1[1] - (dataPtr2 + widthStep2 + nChan2)[1];
                    pixels1[2] = pixels1[2] - (dataPtr2 + widthStep2 + nChan2)[2];

                    //bottom pixels2
                    pixels2[0] += (dataPtr2 + widthStep + nChan2)[0];
                    pixels2[1] += (dataPtr2 + widthStep + nChan2)[1];
                    pixels2[2] += (dataPtr2 + widthStep + nChan2)[2];
                    pixels2[0] += 3 * (dataPtr2 + widthStep)[0];
                    pixels2[1] += 3 * (dataPtr2 + widthStep)[1];
                    pixels2[2] += 3 * (dataPtr2 + widthStep)[2];
                    //top pixels2
                    pixels2[0] = pixels2[0] - (dataPtr2 + nChan2 - widthStep)[0];
                    pixels2[1] = pixels2[1] - (dataPtr2 + nChan2 - widthStep)[1];
                    pixels2[2] = pixels2[2] - (dataPtr2 + nChan2 - widthStep)[2];
                    pixels2[0] = pixels2[0] - 3 * (dataPtr2 - widthStep)[0];
                    pixels2[1] = pixels2[1] - 3 * (dataPtr2 - widthStep)[1];
                    pixels2[2] = pixels2[2] - 3 * (dataPtr2 - widthStep)[2];


                    s[0] = Math.Abs(pixels1[0]) + Math.Abs(pixels2[0]);
                    s[1] = Math.Abs(pixels1[1]) + Math.Abs(pixels2[1]);
                    s[2] = Math.Abs(pixels1[2]) + Math.Abs(pixels2[2]);

                    //check
                    if (s[0] > 255)
                        s[0] = 255;
                    if (s[1] > 255)
                        s[1] = 255;
                    if (s[2] > 255)
                        s[2] = 255;
                    if (s[0] < 0)
                        s[0] = 0;
                    if (s[1] < 0)
                        s[1] = 0;
                    if (s[2] < 0)
                        s[2] = 0;



                    dataPtr[0] = Convert.ToByte(s[0]);
                    dataPtr[1] = Convert.ToByte(s[1]);
                    dataPtr[2] = Convert.ToByte(s[2]);

                    s[0] = 0;
                    s[1] = 0;
                    s[2] = 0;

                    pixels1[0] = 0;
                    pixels1[1] = 0;
                    pixels1[2] = 0;

                    pixels2[0] = 0;
                    pixels2[1] = 0;
                    pixels2[2] = 0;

                    dataPtr -= widthStep;
                    dataPtr2 -= widthStep2;


                }

            }
        }

        internal static void Median(Image<Bgr, byte> img)
        {
            unsafe
            {
                //new copy
                Image<Bgr, Byte> imgcopy = img.Copy();
                MIplImage m2 = imgcopy.MIplImage;
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer();
                int nChan2 = m2.nChannels;
                int padding2 = m2.widthStep - m2.nChannels * m2.width;
                // get the pointer to the beginning of the image 
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int widthStep = m.widthStep;
                int widthStep2 = m2.widthStep;
                int width2 = imgcopy.Width;
                int height2 = imgcopy.Height;

                byte* dataPtrpom = dataPtr;
                byte* dataPtrpom2 = dataPtr2;

                int[] pixels = new int[3];

                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;

                //centre
                dataPtr += widthStep + nChan;
                dataPtr2 += widthStep2 + nChan2;
                int[] values = new int[9];
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        values[0] = Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);
                       
                        values[1] = Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 - widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 - widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 - widthStep2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[2] = Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 - widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 - widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 - widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 + nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[3] = Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 - widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 - widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 - nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 - nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 - nChan2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 - nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[4] = Math.Abs((dataPtr2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 - widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 - widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[5] = Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 + nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 + nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 + nChan2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[6] = Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 - nChan2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[7] = Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 - widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2)[0] - (dataPtr2 + widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        values[8] = Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 - widthStep2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 - widthStep2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 - widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 - widthStep2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 - widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 - widthStep2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 - nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 - nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - dataPtr2[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - dataPtr2[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - dataPtr2[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 + nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 + nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 + nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 + widthStep2 - nChan2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 + widthStep2 - nChan2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2)[0]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[1] - (dataPtr2 + widthStep2)[1]) + Math.Abs((dataPtr2 + widthStep2 + nChan2)[2] - (dataPtr2 + widthStep2)[2])
                            + Math.Abs((dataPtr2 + widthStep2 + nChan2)[0] - (dataPtr2 + widthStep2 - nChan2)[0]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[1] - (dataPtr2 + widthStep2 + nChan2)[1]) + Math.Abs((dataPtr2 - widthStep2 - nChan2)[2] - (dataPtr2 + widthStep2 + nChan2)[2]);

                        int index = Array.IndexOf(values,values.Min());

                        switch (index)
                        {
                            case 0:
                                pixels[0] = (dataPtr2 - widthStep2 - nChan2)[0];
                                pixels[1] = (dataPtr2 - widthStep2 - nChan2)[1];
                                pixels[2] = (dataPtr2 - widthStep2 - nChan2)[2];
                                break;
                            case 1:
                                pixels[0] = (dataPtr2 - widthStep2)[0];
                                pixels[1] = (dataPtr2 - widthStep2)[1];
                                pixels[2] = (dataPtr2 - widthStep2)[2];
                                break;
                            case 2:
                                pixels[0] = (dataPtr2 - widthStep2 + nChan2)[0];
                                pixels[1] = (dataPtr2 - widthStep2 + nChan2)[1];
                                pixels[2] = (dataPtr2 - widthStep2 + nChan2)[2];
                                break;
                            case 3:
                                pixels[0] = (dataPtr2 - nChan2)[0];
                                pixels[1] = (dataPtr2 - nChan2)[1];
                                pixels[2] = (dataPtr2 - nChan2)[2];
                                break;
                            case 4:
                                pixels[0] = dataPtr2[0];
                                pixels[1] = dataPtr2[1];
                                pixels[2] = dataPtr2[2];
                                break;
                            case 5:
                                pixels[0] = (dataPtr2 + nChan2)[0];
                                pixels[1] = (dataPtr2 + nChan2)[1];
                                pixels[2] = (dataPtr2 + nChan2)[2];
                                break;
                            case 6:
                                pixels[0] = (dataPtr2 + widthStep2 - nChan2)[0];
                                pixels[1] = (dataPtr2 + widthStep2 - nChan2)[1];
                                pixels[2] = (dataPtr2 + widthStep2 - nChan2)[2];
                                break;
                            case 7:
                                pixels[0] = (dataPtr2 + widthStep2)[0];
                                pixels[1] = (dataPtr2 + widthStep2)[1];
                                pixels[2] = (dataPtr2 + widthStep2)[2];
                                break;
                            case 8:
                                pixels[0] = (dataPtr2 + widthStep2 + nChan2)[0];
                                pixels[1] = (dataPtr2 + widthStep2 + nChan2)[1];
                                pixels[2] = (dataPtr2 + widthStep2 + nChan2)[2];
                                break;
                            default: break;
                        }
                        dataPtr[0] = Convert.ToByte(pixels[0]);
                        dataPtr[1] = Convert.ToByte(pixels[1]);
                        dataPtr[2] = Convert.ToByte(pixels[2]);
                        pixels[0] = 0;
                        pixels[1] = 0;
                        pixels[2] = 0;
                        dataPtr += nChan;
                        dataPtr2 += nChan2;
                    }
                    dataPtr += 2 * nChan + padding;
                    dataPtr2 += 2 * nChan2 + padding2;
                }
                //top left corner
              /*  dataPtr = dataPtrpom;
                dataPtr2 = dataPtrpom2;
                values[0] = dataPtr2[0];
                values[1] = dataPtr2[0];
                values[2] = (dataPtr2 + nChan2)[0];
                values[3] = dataPtr2[0];
                values[4] = dataPtr2[0];
                values[5] = (dataPtr2 + nChan2)[0];
                values[6] = (dataPtr2 + widthStep2)[0];
                values[7] = (dataPtr2 + widthStep2)[0];
                values[8] = (dataPtr2 + widthStep2 + nChan2)[0];
                Array.Sort<int>(values);
                pixels[0] = values[4];
                values[0] = dataPtr2[1];
                values[1] = dataPtr2[1];
                values[2] = (dataPtr2 + nChan2)[1];
                values[3] = dataPtr2[1];
                values[4] = dataPtr2[1];
                values[5] = (dataPtr2 + nChan2)[1];
                values[6] = (dataPtr2 + widthStep2)[1];
                values[7] = (dataPtr2 + widthStep2)[1];
                values[8] = (dataPtr2 + widthStep2 + nChan2)[1];
                Array.Sort<int>(values);
                pixels[1] = values[4];
                values[0] = dataPtr2[2];
                values[1] = dataPtr2[2];
                values[2] = (dataPtr2 + nChan2)[2];
                values[3] = dataPtr2[2];
                values[4] = dataPtr2[2];
                values[5] = (dataPtr2 + nChan2)[2];
                values[6] = (dataPtr2 + widthStep2)[2];
                values[7] = (dataPtr2 + widthStep2)[2];
                values[8] = (dataPtr2 + widthStep2 + nChan2)[2];
                Array.Sort<int>(values);
                pixels[2] = values[4];
                dataPtr[0] = Convert.ToByte(pixels[0]);
                dataPtr[1] = Convert.ToByte(pixels[1]);
                dataPtr[2] = Convert.ToByte(pixels[2]);
                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;
                //top
                dataPtr += nChan;
                dataPtr2 += nChan2;
                for(int x = 1; x < width2 - 1; x++)
                {
                    values[0] = (dataPtr2 - nChan2)[0];
                    values[1] = dataPtr2[0];
                    values[2] = (dataPtr2 + nChan2)[0];
                    values[3] = (dataPtr2 - nChan2)[0];
                    values[4] = dataPtr2[0];
                    values[5] = (dataPtr2 + nChan2)[0];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[0];
                    values[7] = (dataPtr2 + widthStep2)[0];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[0];
                    Array.Sort<int>(values);
                    pixels[0] = values[4];
                    values[0] = (dataPtr2 - nChan2)[1];
                    values[1] = dataPtr2[1];
                    values[2] = (dataPtr2 + nChan2)[1];
                    values[3] = (dataPtr2 - nChan2)[1];
                    values[4] = dataPtr2[1];
                    values[5] = (dataPtr2 + nChan2)[1];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[1];
                    values[7] = (dataPtr2 + widthStep2)[1];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[1];
                    Array.Sort<int>(values);
                    pixels[1] = values[4];
                    values[0] = (dataPtr2 - nChan2)[2];
                    values[1] = dataPtr2[2];
                    values[2] = (dataPtr2 + nChan2)[2];
                    values[3] = (dataPtr2 - nChan2)[2];
                    values[4] = dataPtr2[2];
                    values[5] = (dataPtr2 + nChan2)[2];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[2];
                    values[7] = (dataPtr2 + widthStep2)[2];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[2];
                    Array.Sort<int>(values);
                    pixels[2] = values[4];
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;
                    dataPtr += nChan;
                    dataPtr2 += nChan2;
                }
                //top wright corner
                values[0] = (dataPtr2 - nChan2)[0];
                values[1] = dataPtr2[0];
                values[2] = dataPtr2[0];
                values[3] = (dataPtr2 - nChan2)[0];
                values[4] = dataPtr2[0];
                values[5] = dataPtr2[0];
                values[6] = (dataPtr2 + widthStep2 - nChan2)[0];
                values[7] = (dataPtr2 + widthStep2)[0];
                values[8] = (dataPtr2 + widthStep2)[0];
                Array.Sort<int>(values);
                pixels[0] = values[4];
                values[0] = (dataPtr2 - nChan2)[1];
                values[1] = dataPtr2[1];
                values[2] = dataPtr2[1];
                values[3] = (dataPtr2 - nChan2)[1];
                values[4] = dataPtr2[1];
                values[5] = dataPtr2[1];
                values[6] = (dataPtr2 + widthStep2)[1];
                values[7] = (dataPtr2 + widthStep2)[1];
                values[8] = (dataPtr2 + widthStep2 + nChan2)[1];
                Array.Sort<int>(values);
                pixels[1] = values[4];
                values[0] = (dataPtr2 - nChan2)[2];
                values[1] = dataPtr2[2];
                values[2] = dataPtr2[2];
                values[3] = (dataPtr2 - nChan2)[2];
                values[4] = dataPtr2[2];
                values[5] = dataPtr2[2];
                values[6] = (dataPtr2 + widthStep2 - nChan2)[2];
                values[7] = (dataPtr2 + widthStep2)[2];
                values[8] = (dataPtr2 + widthStep2)[2];
                Array.Sort<int>(values);
                pixels[2] = values[4];
                dataPtr[0] = Convert.ToByte(pixels[0]);
                dataPtr[1] = Convert.ToByte(pixels[1]);
                dataPtr[2] = Convert.ToByte(pixels[2]);
                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;
                //right
                dataPtr += widthStep;
                dataPtr2 += widthStep2;
                for(int y = 1; y < height2 - 1; y++)
                {
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[0];
                    values[1] = (dataPtr2 - widthStep2)[0];
                    values[2] = (dataPtr2 - widthStep2)[0];
                    values[3] = (dataPtr2 - nChan2)[0];
                    values[4] = dataPtr2[0];
                    values[5] = dataPtr2[0];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[0];
                    values[7] = (dataPtr2 + widthStep2)[0];
                    values[8] = (dataPtr2 + widthStep2)[0];
                    Array.Sort<int>(values);
                    pixels[0] = values[4];
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[1];
                    values[1] = (dataPtr2 - widthStep2)[1];
                    values[2] = (dataPtr2 - widthStep2)[1];
                    values[3] = (dataPtr2 - nChan2)[1];
                    values[4] = dataPtr2[1];
                    values[5] = dataPtr2[1];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[1];
                    values[7] = (dataPtr2 + widthStep2)[1];
                    values[8] = (dataPtr2 + widthStep2)[1];
                    Array.Sort<int>(values);
                    pixels[1] = values[4];
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[2];
                    values[1] = (dataPtr2 - widthStep2)[2];
                    values[2] = (dataPtr2 - widthStep2)[2];
                    values[3] = (dataPtr2 - nChan2)[2];
                    values[4] = dataPtr2[2];
                    values[5] = dataPtr2[2];
                    values[6] = (dataPtr2 + widthStep2 - nChan2)[2];
                    values[7] = (dataPtr2 + widthStep2)[2];
                    values[8] = (dataPtr2 + widthStep2)[2];
                    Array.Sort<int>(values);
                    pixels[2] = values[4];
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;
                    dataPtr += widthStep;
                    dataPtr2 += widthStep2;
                }
                //bottom right corner
                values[0] = (dataPtr2 - widthStep2 - nChan2)[0];
                values[1] = (dataPtr2 - widthStep2)[0];
                values[2] = (dataPtr2 - widthStep2)[0];
                values[3] = (dataPtr2 - nChan2)[0];
                values[4] = dataPtr2[0];
                values[5] = dataPtr2[0];
                values[6] = (dataPtr2- nChan2)[0];
                values[7] = dataPtr2[0];
                values[8] = dataPtr2[0];
                Array.Sort<int>(values);
                pixels[0] = values[4];
                values[0] = (dataPtr2 - widthStep2 - nChan2)[1];
                values[1] = (dataPtr2 - widthStep2)[1];
                values[2] = (dataPtr2 - widthStep2)[1];
                values[3] = (dataPtr2 - nChan2)[1];
                values[4] = dataPtr2[1];
                values[5] = dataPtr2[1];
                values[6] = (dataPtr2- nChan2)[1];
                values[7] = dataPtr2[1];
                values[8] = dataPtr2[1];
                Array.Sort<int>(values);
                pixels[1] = values[4];
                values[0] = (dataPtr2 - widthStep2 - nChan2)[2];
                values[1] = (dataPtr2 - widthStep2)[2];
                values[2] = (dataPtr2 - widthStep2)[2];
                values[3] = (dataPtr2 - nChan2)[2];
                values[4] = dataPtr2[2];
                values[5] = dataPtr2[2];
                values[6] = (dataPtr2- nChan2)[2];
                values[7] = dataPtr2[2];
                values[8] = dataPtr2[2];
                Array.Sort<int>(values);
                pixels[2] = values[4];
                dataPtr[0] = Convert.ToByte(pixels[0]);
                dataPtr[1] = Convert.ToByte(pixels[1]);
                dataPtr[2] = Convert.ToByte(pixels[2]);
                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;
                //bottom
                dataPtr -= nChan;
                dataPtr2 -= nChan2;
                for(int x = 1; x < width2 - 1; x++)
                {
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[0];
                    values[1] = (dataPtr2 - widthStep2)[0];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[0];
                    values[3] = (dataPtr2 - nChan2)[0];
                    values[4] = dataPtr2[0];
                    values[5] = (dataPtr2 + nChan2)[0];
                    values[6] = (dataPtr2 - nChan2)[0];
                    values[7] = dataPtr2[0];
                    values[8] = (dataPtr2 + nChan2)[0];
                    Array.Sort<int>(values);
                    pixels[0] = values[4];
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[1];
                    values[1] = (dataPtr2 - widthStep2)[1];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[1];
                    values[3] = (dataPtr2 - nChan2)[1];
                    values[4] = dataPtr2[1];
                    values[5] = (dataPtr2 + nChan2)[1];
                    values[6] = (dataPtr2 - nChan2)[1];
                    values[7] = dataPtr2[1];
                    values[8] = (dataPtr2 + nChan2)[1];
                    Array.Sort<int>(values);
                    pixels[1] = values[4];
                    values[0] = (dataPtr2 - widthStep2 - nChan2)[2];
                    values[1] = (dataPtr2 - widthStep2)[2];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[2];
                    values[3] = (dataPtr2 - nChan2)[2];
                    values[4] = dataPtr2[2];
                    values[5] = (dataPtr2 + nChan2)[2];
                    values[6] = (dataPtr2 - nChan2)[2];
                    values[7] = dataPtr2[2];
                    values[8] = (dataPtr2 + nChan2)[2];
                    Array.Sort<int>(values);
                    pixels[2] = values[4];
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;
                    dataPtr -= nChan;
                    dataPtr2 -= nChan2;
                }
                //bottom left corner
                values[0] = (dataPtr2 - widthStep2)[0];
                values[1] = (dataPtr2 - widthStep2)[0];
                values[2] = (dataPtr2 - widthStep2 + nChan2)[0];
                values[3] = dataPtr2[0];
                values[4] = dataPtr2[0];
                values[5] = (dataPtr2 + nChan2)[0];
                values[6] = dataPtr2[0];
                values[7] = dataPtr2[0];
                values[8] = (dataPtr2 + nChan2)[0];
                Array.Sort<int>(values);
                pixels[0] = values[4];
                values[0] = (dataPtr2 - widthStep2)[1];
                values[1] = (dataPtr2 - widthStep2)[1];
                values[2] = (dataPtr2 - widthStep2 + nChan2)[1];
                values[3] = dataPtr2[1];
                values[4] = dataPtr2[1];
                values[5] = (dataPtr2 + nChan2)[1];
                values[6] = dataPtr2[1];
                values[7] = dataPtr2[1];
                values[8] = (dataPtr2 + nChan2)[1];
                Array.Sort<int>(values);
                pixels[1] = values[4];
                values[0] = (dataPtr2 - widthStep2)[2];
                values[1] = (dataPtr2 - widthStep2)[2];
                values[2] = (dataPtr2 - widthStep2 + nChan2)[2];
                values[3] = dataPtr2[2];
                values[4] = dataPtr2[2];
                values[5] = (dataPtr2 + nChan2)[2];
                values[6] = dataPtr2[2];
                values[7] = dataPtr2[2];
                values[8] = (dataPtr2 + nChan2)[2];
                Array.Sort<int>(values);
                pixels[2] = values[4];
                dataPtr[0] = Convert.ToByte(pixels[0]);
                dataPtr[1] = Convert.ToByte(pixels[1]);
                dataPtr[2] = Convert.ToByte(pixels[2]);
                pixels[0] = 0;
                pixels[1] = 0;
                pixels[2] = 0;

                //left
                dataPtr -= widthStep;
                dataPtr2 -= widthStep2;
                for(int y = 1; y < height2 - 1; y++)
                {
                    values[0] = (dataPtr2 - widthStep2)[0];
                    values[1] = (dataPtr2 - widthStep2)[0];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[0];
                    values[3] = dataPtr2[0];
                    values[4] = dataPtr2[0];
                    values[5] = (dataPtr2 + nChan2)[0];
                    values[6] = (dataPtr2 + widthStep2)[0];
                    values[7] = (dataPtr2 + widthStep2)[0];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[0];
                    Array.Sort<int>(values);
                    pixels[0] = values[4];
                    values[0] = (dataPtr2 - widthStep2)[1];
                    values[1] = (dataPtr2 - widthStep2)[1];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[1];
                    values[3] = dataPtr2[1];
                    values[4] = dataPtr2[1];
                    values[5] = (dataPtr2 + nChan2)[1];
                    values[6] = (dataPtr2 + widthStep2)[1];
                    values[7] = (dataPtr2 + widthStep2)[1];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[1];
                    Array.Sort<int>(values);
                    pixels[1] = values[4];
                    values[0] = (dataPtr2 - widthStep2)[2];
                    values[1] = (dataPtr2 - widthStep2)[2];
                    values[2] = (dataPtr2 - widthStep2 + nChan2)[2];
                    values[3] = dataPtr2[2];
                    values[4] = dataPtr2[2];
                    values[5] = (dataPtr2 + nChan2)[2];
                    values[6] = (dataPtr2 + widthStep2)[2];
                    values[7] = (dataPtr2 + widthStep2)[2];
                    values[8] = (dataPtr2 + widthStep2 + nChan2)[2];
                    Array.Sort<int>(values);
                    pixels[2] = values[4];
                    dataPtr[0] = Convert.ToByte(pixels[0]);
                    dataPtr[1] = Convert.ToByte(pixels[1]);
                    dataPtr[2] = Convert.ToByte(pixels[2]);
                    pixels[0] = 0;
                    pixels[1] = 0;
                    pixels[2] = 0;
                    dataPtr -= widthStep;
                    dataPtr2 -= widthStep2;
                }*/

            }
        }


    }
}

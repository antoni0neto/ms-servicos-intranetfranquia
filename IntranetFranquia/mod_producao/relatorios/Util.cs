using GenCode128;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Relatorios.mod_producao.relatorios
{
    public static class Util
    {
        public static void GerarCodigoBarras(string diretorioPadrao, string nomeCodigoBarras)
        {
            // definir tamanho do código de barras
            int w = nomeCodigoBarras.Length * 32;

            Bitmap oBitmap = new Bitmap(945, 213);
            Graphics oGraphics = Graphics.FromImage(oBitmap);

            // criar fonte para gerar código de barras
            //Font oFont = new Font("IDAutomationHC39M", 18);
            //Font oFont = new Font("IDAHC39MCode39Barcode", 18);
            Font oFont = new Font("MRV Code39extMA", 36);

            PointF oPoint = new PointF(2f, 2f);
            SolidBrush oBrushWrite = new SolidBrush(Color.Black);
            SolidBrush oBrush = new SolidBrush(Color.White);

            oGraphics.FillRectangle(oBrush, 0, 0, 945, 213);

            // gerar codigo de barra
            oGraphics.DrawString(" " + nomeCodigoBarras + " ", oFont, oBrushWrite, oPoint);

            string imageLocation = diretorioPadrao + "\\Image_BARCODE\\" + nomeCodigoBarras + ".png";
            oBitmap.Save(imageLocation, ImageFormat.Png);
        }

        public static void GerarCodigoBarrasCode128DLL(string diretorioPadrao, string nomeCodigoBarras)
        {
            Image myimg = Code128Rendering.MakeBarcodeImage(nomeCodigoBarras, 2, true);

            Bitmap oBitmap = new Bitmap(945, 213);

            string imageLocation = diretorioPadrao + "\\Image_BARCODE\\" + nomeCodigoBarras + ".png";
            myimg.Save(imageLocation, ImageFormat.Png);
        }
    }
}
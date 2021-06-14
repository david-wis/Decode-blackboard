using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Programa para generar fotos a partir del mathml del editor de blackboard mediante el servicio de wiris
/// Creado con la intención de recuperar las respuestas del segundo parcial de la materia de Análisis Matemático I del ITBA 
/// que no se visualizan correctamente por un bug del editor de ecuaciones de blackboard
/// 
/// Autor: David Wischñevsky
/// </summary>
namespace Decode_blackboard
{

    //https://www.wiris.net/demo/editor/
    public partial class Form1 : Form
    {
        public const string url = @"https://www.wiris.net/demo/editor/render";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string rawText = txtBox.Text;
            if (txtBox.Text.Length > 0)
            {
                // Primero se cambian los « y » por < y >, y los ¨ por ' y para que el servicio pueda leer el mathml
                // Además, se reemplazan los signos + por su representacion para urls (%2B)
                string newText = rawText.Replace("»", ">").Replace("«", "<").Replace("¨", "'").Replace("+", "%2B");

                // Luego se reemplazan los caracteres especiales por sus respectivas encodificaciones en unicode
                // Esto permite que caracteres como el ∞ se puedan cargar en la imagen
                string cleanedString = Regex.Replace(newText, @"§#[0-9]+;", m => {
                    string aux = m.Value.Replace("§", "&");
                    return System.Net.WebUtility.HtmlDecode(aux);
                });

                try
                {
                    picture.Load($"{url}?mml={cleanedString}"); // Llamado al servicio de wiris.net para generar la foto
                    Console.WriteLine($"{url}?mml={cleanedString}");
                } 
                catch (Exception)
                {
                    MessageBox.Show("La cadena de texto ingresada no es correcta o se produjo un error generando la imagen");
                }
            } else
            {
                MessageBox.Show("Por favor ingrese algún valor en la caja de texto");
            }
        }
    }
}

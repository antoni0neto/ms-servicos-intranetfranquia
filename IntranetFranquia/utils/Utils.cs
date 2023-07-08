using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public static class Utils
{
    public static class WebControls
    {
        public static bool ValidarEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static void GetBoundFieldIndexByName(GridView gv, string replaceText)
        {
            if (gv.Attributes["HeaderText"] == null)
                gv.Attributes.Add("HeaderText", "");

            if (gv.Attributes["ChangeTextHeader"] == null)
                gv.Attributes.Add("ChangeTextHeader", "");

            foreach (DataControlField c in gv.Columns)
                if (c != null)
                    c.HeaderText = c.HeaderText.Replace(replaceText, "");

        }
        public static void GetBoundFieldIndexByName(GridView gv, string name, string changeTextHeader)
        {
            if (gv.Attributes["HeaderText"] == null)
                gv.Attributes.Add("HeaderText", "");

            if (gv.Attributes["ChangeTextHeader"] == null)
                gv.Attributes.Add("ChangeTextHeader", "");

            foreach (DataControlField c in gv.Columns)
                if (c is BoundField || c is TemplateField)
                    if (gv.Attributes["ChangeTextHeader"].Length > 0)
                        c.HeaderText = c.HeaderText.Replace(gv.Attributes["ChangeTextHeader"], "");

            foreach (DataControlField c in gv.Columns)
            {
                if (c is BoundField || c is TemplateField)

                    if (name == c.SortExpression ||
                        name == c.HeaderText)
                    {
                        gv.Attributes["HeaderText"] = c.HeaderText;
                        gv.Attributes["ChangeTextHeader"] = changeTextHeader;

                        if (!string.IsNullOrEmpty(changeTextHeader))
                            c.HeaderText = gv.Attributes["HeaderText"].ToString() + changeTextHeader;

                        break;
                    }
            }
        }
        public static void GridViewSortDirection(GridView g, GridViewSortEventArgs e, out SortDirection d)
        {
            d = e.SortDirection;

            if (g.Attributes["CurrentSortField"] != null && g.Attributes["CurrentSortDir"] != null)
            {
                if (e.SortExpression == g.Attributes["CurrentSortField"])
                {
                    d = SortDirection.Descending;

                    if (g.Attributes["CurrentSortDir"] == "ASC")
                    {
                        d = SortDirection.Ascending;
                    }
                }

                g.Attributes["CurrentSortField"] = e.SortExpression;
                g.Attributes["CurrentSortDir"] = (d == SortDirection.Ascending ? "DESC" : "ASC");
            }
            else
            {
                g.Attributes.Add("CurrentSortField", e.SortExpression);
                g.Attributes.Add("CurrentSortDir", "");
            }
        }
        public static void LimparControles(ref List<ControlesValidacao> listControles)
        {
            if (listControles != null)
            {
                foreach (var item in listControles)
                {
                    Control c = item.Controle;

                    Label l = item.Label;
                    if (l != null)
                    {
                        l.ForeColor = item.CorPadrao;
                    }

                    if (c.GetType().Name == "TextBox")
                    {
                        TextBox t = c as TextBox;

                        t.Text = item.ValorPadrao;
                    }
                    else if (c.GetType().Name == "Label")
                    {
                        Label lab = c as Label;

                        lab.Text = item.ValorPadrao;
                    }
                    else if (c.GetType().Name == "DropDownList")
                    {
                        DropDownList ddl = c as DropDownList;

                        if (ddl.Items.Count > 0)
                        {
                            ddl.SelectedIndex = -1;
                        }
                    }
                    else if (c.GetType().Name == "ListBox")
                    {
                        ListBox lbox = c as ListBox;

                        if (lbox.Items.Count > 0)
                        {
                            lbox.SelectedIndex = -1;
                        }
                    }
                }
            }
        }
        public static bool ValidarCampos(ref List<ControlesValidacao> listControles)
        {
            bool validado = true;

            if (listControles != null)
            {
                foreach (var item in listControles)
                {
                    Control c = item.Controle;

                    Label l = item.Label;
                    if (l != null)
                        l.ForeColor = item.CorPadrao;

                    if (c.GetType().Name == "TextBox")
                    {
                        TextBox txt = c as TextBox;

                        if (txt.Text == item.ValorPadrao)
                        {
                            if (l != null)
                                l.ForeColor = item.CorAviso;

                            validado = false;
                        }

                        if (item.TamanhoMinimo > 0 && item.TamanhoMaximo > 0)
                            if (txt.Text.Length < item.TamanhoMinimo || txt.Text.Length > item.TamanhoMaximo)
                            {
                                if (l != null)
                                    l.ForeColor = item.CorAviso;

                                validado = false;
                            }
                    }
                    else if (c.GetType().Name == "DropDownList")
                    {
                        DropDownList ddl = c as DropDownList;

                        if (ddl.Items.Count > 0)
                        {
                            if (ddl.SelectedItem.Text == item.ValorPadrao)
                            {
                                if (l != null)
                                    l.ForeColor = item.CorAviso;

                                validado = false;
                            }

                            if (ddl.SelectedValue == item.ValorPadrao)
                            {
                                if (l != null)
                                    l.ForeColor = item.CorAviso;

                                validado = false;
                            }
                        }
                        else
                        {
                            if (l != null)
                                l.ForeColor = item.CorAviso;

                            validado = false;
                        }
                    }
                    else if (c.GetType().Name == "ListBox")
                    {
                        ListBox lbox = c as ListBox;

                        if (lbox.Items.Count > 0)
                        {
                            if (lbox.SelectedItem.Text == item.ValorPadrao)
                            {
                                if (l != null)
                                    l.ForeColor = item.CorAviso;

                                validado = false;
                            }

                            if (lbox.SelectedValue == item.ValorPadrao)
                            {
                                if (l != null)
                                    l.ForeColor = item.CorAviso;

                                validado = false;
                            }
                        }
                        else
                        {
                            if (l != null)
                                l.ForeColor = item.CorAviso;

                            validado = false;
                        }
                    }
                }
            }
            return validado;
        }
        public static Control FindControlRecursive(Control ctlRoot, string sControlId)
        {
            // if this control is the one we are looking for, break from the recursion
            // and return the control.
            if (ctlRoot.ID == sControlId)
            {
                return ctlRoot;
            }

            // loop the child controls of this parent control and call recursively.
            foreach (Control ctl in ctlRoot.Controls)
            {
                Control ctlFound = FindControlRecursive(ctl, sControlId);

                // if we found the control, return it.
                if (ctlFound != null)
                {
                    return ctlFound;
                }
            }

            // we never found the control so just return null.
            return null;
        }
        public class ControlesValidacao
        {
            public Label Label { get; set; }
            public Control Controle { get; set; }
            public Color CorPadrao { get; set; }
            public Color CorAviso { get; set; }
            public string ValorPadrao { get; set; }
            public int TamanhoMinimo { get; set; }
            public int TamanhoMaximo { get; set; }
        }

        public static bool ValidarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string AlterarPrimeiraLetraMaiscula(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            //Obter index do hifen
            int indexHifen = source.IndexOf('-');

            //Converte para array de char[]
            char[] letters = source.ToCharArray();

            //Upper na primeira letra
            letters[0] = char.ToUpper(letters[0]);

            //Upper na Segunda letra
            if (indexHifen > 0)
                letters[indexHifen + 1] = char.ToUpper(letters[indexHifen + 1]);

            //Retorna a String
            return new string(letters);
        }

        public static int ObterTotalDiasUteis(DateTime initialDate, DateTime finalDate)
        {
            int days = 0;
            int daysCount = 0;

            days = initialDate.Subtract(finalDate.AddDays(1)).Days;
            //Módulo
            if (days < 0)
                days = days * -1;

            for (int i = 1; i <= days; i++)
            {
                //Conta apenas dias da semana.
                if (initialDate.DayOfWeek != DayOfWeek.Sunday &&
                    initialDate.DayOfWeek != DayOfWeek.Saturday)
                    daysCount++;

                initialDate = initialDate.AddDays(1);
            }

            return daysCount;
        }

    }

    public class SessionManager
    {
        public static void Armazenar<T>(T obj, string name)
        {
            HttpContext.Current.Session[name] = obj;
        }
        public static T Obter<T>(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }
        public static void Limpar(string name)
        {
            HttpContext.Current.Session[name] = null;
        }
    }

}

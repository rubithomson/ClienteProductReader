using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class MasterPage : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            actualizarGrid();

        if (!IsPostBack)
            actualizarSelectDto();
    }

    


    ServiceReference1.Service1Client Cliente = new ServiceReference1.Service1Client();

 //------------ INICIA TAB AGREGAR PRODUCTO--------------------------

    private void actualizarSelectDto()
    {
        var registro = Cliente.ListarDepartamentos();
        foreach (var depto in registro)
        {

            string n = depto.nombre;

            selectDtoAgregar.Items.Add(n);
            selectDtoModificar.Items.Add(n);

        }
    }
    protected void btnGuardarProducto_Click(object sender, EventArgs e)
    {

        string codbar = txtCodBarAgregar.Text;
        string nombre = txtNombreAgregar.Text;
        string descripcion = txtDesAgregar.Text;
        string valor = selectUMAgregar.Value;
        string um = ""; int b = 3;
        //if (valor == "0") txtCodBarAgregar.Text = um;
        if (valor == "1") um = "pza";
        else if (valor == "2") um = "kg";
        else b = 0;
        string spv = txtPVAgregar.Text;
        float pv = float.Parse(spv);
        //string dep = selectDtoAgregar.SelectedIndex;
        int nDep = selectDtoAgregar.SelectedIndex;

        if (Cliente.GuardarProducto(codbar, nombre, descripcion, um, pv, "activo", nDep) && b>0)
        {
            //Console.WriteLine("True");
            //Console.ReadKey();
            txtCodBarAgregar.Text = "";
            txtNombreAgregar.Text = "";
            txtDesAgregar.Text = "";
            selectUMAgregar.SelectedIndex = 0;
            txtPVAgregar.Text = "";
            selectDtoAgregar.SelectedIndex = 0;

        }





    }


    //------------ TERMINAR TAB AGREGAR PRODUCTO--------------------------


//------------ INICIAR TAB MODIFICAR PRODUCTO--------------------------

    protected void btnBuscarModificar_Click(object sender, EventArgs e)
    {
        string codbar = "";
        codbar = txtCodBarModificar.Text;
        if (codbar.Length > 0)
        {
            try
            {
                string producto = Cliente.BuscarProducto(codbar);
                string[] separadas;
                separadas = producto.Split('¬');

                txtNombreModificar.Text = separadas[1];
                txtDescModificar.Text = separadas[2];

                if (separadas[3] == "pza")
                    selectUMModificar.SelectedIndex = 1;
                else selectUMModificar.SelectedIndex = 2;

                txtPreVenModificar.Text = separadas[4];

                selectDtoModificar.SelectedIndex = Int32.Parse(separadas[6]);
            }
            catch(Exception ex)
            {
                string error = "No existe el producto";
                txtNombreModificar.Text = error;
                txtDescModificar.Text = "";
                selectUMModificar.SelectedIndex = 0;
                txtPreVenModificar.Text = "";
                selectDtoModificar.SelectedIndex = 0;
            }
            


        }
    }

 //------------ TERMINAR TAB MODIFICAR PRODUCTO--------------------------


    //------------ INICIA TAB DEPARTAMENTOS--------------------------

    private void actualizarGrid()
    {

        var registro = Cliente.ListarDepartamentos();
        listaDepto.DataSource = registro;
        listaDepto.DataBind();
            
        
    }

    protected void btnAgregarDep_Click(object sender, EventArgs e)
    {
        
        string nomDep = txtNomDept.Text;
        
            if (Cliente.AgregarDepartamento(nomDep, "Activo"))
            {
                txtNomDept.Text = "";
            }
                   
    }
    protected void btnGuardarModificar_Click(object sender, EventArgs e)
    {

    }


    // ----------- TERMINA TAB DEPARTAMENTOS ------------------------


    protected void btnUpload_Click(object sender, EventArgs e)
    {

        
            byte[] fileToSend = null;
            string name = "";

            if (img1Agregar.HasFile)
            {
                name = img1Agregar.FileName;
                Stream stream = img1Agregar.FileContent;
                stream.Seek(0, SeekOrigin.Begin);
                fileToSend = new byte[stream.Length];
                int count = 0;
                while (count < stream.Length)
                {
                    fileToSend[count++] = Convert.ToByte(stream.ReadByte());
                }
            Stream stream1 = new MemoryStream(fileToSend);
            Cliente.SubirImagen(stream1);

            }

            /*
            //Check file is selected or not  System.ServiceModel.FaultException`1: 'A generic error occurred in GDI+.'
            if (FileUpload1.HasFile)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                //To Service  
                #region Upload Files  

                string filename = FileUpload1.FileName;
                byte[] filebyte = FileUpload1.FileBytes;

                //Service refrence          
                //ServiceReference.ServiceClient objAttachment = new ServiceReference.ServiceClient();
                ServiceReference1.Service1Client objAttachment = new ServiceReference1.Service1Client();


                //pass the file byte to service  
                objAttachment.UploadToTempFolder(filebyte, filename);
                #endregion
                timer.Stop();
                lblTimeStarts.Text = "File upload takes >>>" + Convert.ToString((TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds).Milliseconds)) + " milliseconds. <br>";

                writetheFileNameInText(filename, "output");
                //bindUploadFile(DataList1, "output");
            }
            */
        }



    private void writetheFileNameInText(string fileName, string outputFileName)
    {

        using (StreamWriter w = File.AppendText(Server.MapPath(@"~\App_Data\" + outputFileName + ".txt")))
        {
            w.WriteLine(fileName);
        }
    }
    /*
    private void bindUploadFile(DataList dl, string outputFileName)
    {
        List<string> lstfileName = new List<string>();
        lstfileName = gettheFileNamefromText(outputFileName);

        dl.DataSource = lstfileName;
        dl.DataBind();

    } */

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }








}

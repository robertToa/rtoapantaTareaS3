using System.Globalization;

namespace rtoapantaTareaS3.Views;

public partial class vResumen : ContentPage
{
	private const decimal TasaAporteIess = 0.0945m;
	private readonly string _resumen;

	public vResumen(
		string tipoIdentificacion,
		string identificacion,
		string nombres,
		string apellidos,
		DateTime fecha,
		string correo,
		string salario)
	{
		InitializeComponent();
		var salarioNumerico = decimal.Parse(salario, NumberStyles.Number, CultureInfo.CurrentCulture);
		var aporteIess = salarioNumerico * TasaAporteIess;

		TipoIdentificacionLabel.Text = $"Tipo de identificación: {tipoIdentificacion}";
		IdentificacionLabel.Text = $"Identificación: {identificacion}";
		NombresLabel.Text = $"Nombres: {nombres}";
		ApellidosLabel.Text = $"Apellidos: {apellidos}";
		FechaLabel.Text = $"Fecha: {fecha:dd/MM/yyyy}";
		CorreoLabel.Text = $"Correo: {correo}";
		SalarioLabel.Text = $"Salario: {salarioNumerico.ToString("N2", CultureInfo.CurrentCulture)}";
		this.FindByName<Label>("IessLabel")!.Text =
			$"Aporte personal al IESS (9,45 %): {aporteIess.ToString("N2", CultureInfo.CurrentCulture)}";

		_resumen = $"Resumen de datos{Environment.NewLine}" +
			$"Tipo de identificación: {tipoIdentificacion}{Environment.NewLine}" +
			$"Identificación: {identificacion}{Environment.NewLine}" +
			$"Nombres: {nombres}{Environment.NewLine}" +
			$"Apellidos: {apellidos}{Environment.NewLine}" +
			$"Fecha: {fecha:dd/MM/yyyy}{Environment.NewLine}" +
			$"Correo: {correo}{Environment.NewLine}" +
			$"Salario: {salarioNumerico.ToString("N2", CultureInfo.CurrentCulture)}{Environment.NewLine}" +
			$"Aporte personal al IESS (9,45 %): {aporteIess.ToString("N2", CultureInfo.CurrentCulture)}";
	}

	private async void OnExportarClicked(object sender, EventArgs e)
	{
		try
		{
			var carpeta = Path.Combine(AppContext.BaseDirectory, "carpeta");
			Directory.CreateDirectory(carpeta);

			var rutaArchivo = Path.Combine(carpeta, "resumen.txt");
			await File.WriteAllTextAsync(rutaArchivo, _resumen);

			await DisplayAlert("Exportación completada", $"Archivo guardado en:{Environment.NewLine}{rutaArchivo}", "Aceptar");
		}
		catch (IOException)
		{
			await DisplayAlert("Error", "No se pudo crear o guardar el archivo de resumen.", "Aceptar");
		}
	}
}
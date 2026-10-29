using System.Globalization;
using System.Text.RegularExpressions;

namespace rtoapantaTareaS3.Views;

public partial class vContacto : ContentPage
{
	public vContacto()
	{
		InitializeComponent();
		TipoIdentificacionPicker.ItemsSource = new[] { "CI", "RUC", "Pasaporte" };
	}

	private async void OnResumenClicked(object sender, EventArgs e)
	{
		var error = ValidarFormulario();
		if (error is not null)
		{
			await DisplayAlert("Validación", error, "Aceptar");
			return;
		}

		await Navigation.PushAsync(new vResumen(
			TipoIdentificacionPicker.SelectedItem!.ToString()!,
			IdentificacionEntry.Text!.Trim(),
			NombresEntry.Text!.Trim(),
			ApellidosEntry.Text!.Trim(),
			FechaPicker.Date ?? DateTime.Today,
			CorreoEntry.Text!.Trim(),
			SalarioEntry.Text!.Trim()));
	}

	private string? ValidarFormulario()
	{
		if (TipoIdentificacionPicker.SelectedItem is null ||
			string.IsNullOrWhiteSpace(IdentificacionEntry.Text) ||
			string.IsNullOrWhiteSpace(NombresEntry.Text) ||
			string.IsNullOrWhiteSpace(ApellidosEntry.Text) ||
			string.IsNullOrWhiteSpace(CorreoEntry.Text) ||
			string.IsNullOrWhiteSpace(SalarioEntry.Text))
		{
			return "Todos los campos son obligatorios.";
		}

		var tipo = TipoIdentificacionPicker.SelectedItem.ToString();
		var identificacion = IdentificacionEntry.Text.Trim();
		if (tipo == "CI" && !Regex.IsMatch(identificacion, "^\\d{10}$"))
		{
			return "La CI debe contener exactamente 10 números.";
		}

		if (tipo == "RUC" && !Regex.IsMatch(identificacion, "^\\d{13}$"))
		{
			return "El RUC debe contener exactamente 13 números.";
		}

		if (!MailEsValido(CorreoEntry.Text.Trim()))
		{
			return "Ingrese un correo electrónico válido.";
		}

		if (!decimal.TryParse(SalarioEntry.Text.Trim(), NumberStyles.Number,
			CultureInfo.CurrentCulture, out _))
		{
			return "El salario debe ser un valor numérico válido.";
		}

		return null;
	}

	private static bool MailEsValido(string correo)
	{
		return Regex.IsMatch(correo,
			@"^[^\s@]+@[^\s@]+\.[^\s@]+$",
			RegexOptions.CultureInvariant);
	}
}
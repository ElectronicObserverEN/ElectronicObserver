using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Core.Types;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Avalonia.Dialogs.ShipSelector;

public partial class ShipSelectorViewModel : ObservableObject, IModalDialogViewModel, ICloseable
{
	public Controls.ShipFilter.ShipFilterViewModel ShipFilter { get; } = new(new(new ConfigurationUi()));
	private List<ShipViewModel> Ships { get; }

	public DataGridCollectionView CollectionView { get; }

	/// <inheritdoc />
	public event EventHandler? RequestClose;

	/// <inheritdoc />
	public bool? DialogResult { get; protected set; }

	[MemberNotNullWhen(true, nameof(DialogResult))]
	public IShipData? SelectedShip { get; set; }

	/// <inheritdoc/>
	public ShipSelectorViewModel(ImageLoadService imageLoadService, List<IShipData> ships)
	{
		Ships = ships
			.Where(s => !s.MasterShip.IsAbyssalShip)
			.OrderBy(s => s.SortID)
			.Select(s => new ShipViewModel(s, imageLoadService))
			.ToList();
		
		CollectionView = new(Ships)
		{
			Filter = o => o switch
			{
				ShipViewModel viewModel => ShipFilter.MeetsFilterCondition(viewModel.Ship),
				_ => true,
			},
		};

		ShipFilter.PropertyChanged += ShipFilter_PropertyChanged;
	}

	private void ShipFilter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		CollectionView.Refresh();
	}

	[RelayCommand]
	private void SelectShip(ShipViewModel? ship)
	{
		SelectedShip = ship?.Ship;
		DialogResult = ship is not null;

		Close();
	}

	protected void Close()
	{
		// https://github.com/AvaloniaUI/Avalonia/issues/16199#issuecomment-2244891047
		Dispatcher.UIThread.Post(() => RequestClose?.Invoke(this, EventArgs.Empty));
	}
}

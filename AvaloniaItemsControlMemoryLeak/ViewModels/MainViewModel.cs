using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Threading;
using AvaloniaItemsControlMemoryLeak.Models;

namespace AvaloniaItemsControlMemoryLeak.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ulong _index;
    private bool _isRunnig;

    public MainViewModel()
    {
        StartCommand = MiniCommand.CreateFromTask(async () =>
        {
            _isRunnig = true;

            while (_isRunnig)
            {
                _index++;
                var index = _index;

                Dispatcher.UIThread.Post(() => Items.Add(new()
                {
                    Id = index,
                    Name = $"Item {index}",
                }));

                if (_index % 100 == 0)
                {
                    Dispatcher.UIThread.Post(() => Items.Clear(), DispatcherPriority.Background);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        });

        StopCommand = MiniCommand.Create(() => _isRunnig = false);
    }

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }
    public AvaloniaList<Item> Items { get; } = new();
}
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Issue_WinForms_FormattedValue;

/// <summary>
///  Human can have only one kitten.
/// </summary>
public class Human : INotifyPropertyChanged {
    private Kitten? _kitten = new();

    public Kitten? Kitten {
        get => _kitten;

        set {
            Console.WriteLine($"Setting Kitten to {value?.Id}");
            _kitten = value;
            OnPropertyChanged(nameof(Kitten));
            OnPropertyChanged(nameof(KittenId));
        }
    }

    public string? KittenId => Kitten?.Id;

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}

/// <summary>
/// Kitten has a name and an id.
/// </summary>
public class Kitten {
    public string? Name { get; set; }
    public string? Id   { get; set; } = Guid.NewGuid().ToString();

    public Kitten  Self        => this;
    public string? DisplayName => $"{Name}";
}

public partial class Form1 : Form {
    // form displays data gridview with humans
    public BindingList<Human> Humans = new();

    // each human can select a kitten from the list
    public BindingList<Kitten> Kittens = new(new[] {
        new Kitten() {
            Name = "Kitty1",
        },
        new Kitten() {
            Name = "Kitty1",
        },
        new Kitten() {
            Name = "Kitty1",
        },
    });

    public DataGridView dataGridView1;

    public Form1() {
        InitializeComponent();
        dataGridView1 = new DataGridView();
        Controls.Add(dataGridView1);
        
        var _kittensColumns = new DataGridViewComboBoxColumn {
            DataPropertyName = "Kitten",
            HeaderText       = "Kitten",
            Name             = "Kitten",
            ValueType        = typeof(Kitten),
            DisplayMember    = "DisplayName",
            ValueMember      = "Self",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.DisplayedCells,
            DisplayStyle     = DataGridViewComboBoxDisplayStyle.ComboBox,
        };

        
        var _kittensColumns2 = new DataGridViewComboBoxColumn {
            DataPropertyName = "Kitten",
            HeaderText       = "Kitten",
            Name             = "Kitten",
            ValueType        = typeof(Kitten),
            DisplayMember    = "Id",
            ValueMember      = "Self",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.DisplayedCells,
            DisplayStyle     = DataGridViewComboBoxDisplayStyle.ComboBox,
        };
        
        dataGridView1.Columns.Add(_kittensColumns);
        dataGridView1.Columns.Add(_kittensColumns2);
        
        _kittensColumns.DataSource = Kittens;
        _kittensColumns2.DataSource = Kittens;
        
        dataGridView1.DataSource   = Humans;
        dataGridView1.Dock = DockStyle.Fill;
        
        Humans.AllowEdit   = true;
        Humans.AllowNew    = false;
        
        // add a human with a kitten
        Humans.Add(new Human { Kitten = Kittens[0] });
    }
}
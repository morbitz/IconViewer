﻿using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace IconViewer.Logic
{
    public class ColorManager : INotifyPropertyChanged
    {
        private List<Icon> Icons;
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool isValid = false;
        public bool IsValid
        {
            get => isValid;
            set
            {
                if (isValid == value)
                {
                    return;
                }

                isValid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }

        private string color;
        public string Color
        {
            get => IsValid ? color : "#fff";
            set
            {
                if (color == value)
                {
                    return;
                }

                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                if (text == value)
                {
                    return;
                }

                Validate(value);
                text = value;

                if (IsValid)
                {
                    Color = text;
                }
            }
        }

        private void Validate(string value)
        {
            try
            {
                _ = (Color)ColorConverter.ConvertFromString(value);
            }
            catch (Exception)
            {
                IsValid = false;
                return;
            }

            IsValid = true;
        }

        private readonly ICommand colorChangedCommand;
        public ICommand ColorChangedCommand => colorChangedCommand ?? new RelayCommand(
                    x => true,
                    x => SetPathColor()
                    );

        public ColorManager()
        {

        }

        public ColorManager(List<Icon> icons)
        {
            Icons = icons ?? throw new ArgumentNullException(nameof(icons));
            Validate(Text);
            SetPathColor();
        }

        public ColorManager(string color)
        {
            Color = color;
            Text = color;
        }

        public ColorManager(List<Icon> icons, string color)
        {
            Icons = icons ?? throw new ArgumentNullException(nameof(icons));
            Text = color;
            Validate(Text);
            SetPathColor();
        }


        internal void SetIcons(IEnumerable<Icon> icons)
        {
            if (icons is null)
            {
                throw new ArgumentNullException(nameof(icons));
            }

            Icons = icons.ToList();
        }

        private void SetPathColor()
        {
            if (Icons.Count == 0)
            {
                return;
            }

            Icons.ForEach(icon =>
            {
                icon.PathColor = Color;
                icon.RisePropertyChanged(nameof(icon.PathColor));
            });
        }
    }
}

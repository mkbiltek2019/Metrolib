﻿using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Metrolib.Controls.Base
{
	/// <summary>
	///     An extended toggle button that offers an IsInverted property to control how the content shall be displayed.
	/// </summary>
	public class FlatToggleButton
		: ToggleButton
	{
		public static readonly DependencyProperty IsInvertedProperty =
			DependencyProperty.Register("IsInverted", typeof (bool), typeof (FlatToggleButton),
			                            new PropertyMetadata(false));

		public static readonly DependencyProperty InvertedForegroundProperty =
			DependencyProperty.Register("InvertedForeground", typeof (Brush), typeof (FlatToggleButton),
			                            new PropertyMetadata(default(Brush)));

		public static readonly DependencyProperty HoveredBackgroundProperty =
			DependencyProperty.Register("HoveredBackground", typeof(Brush), typeof(FlatToggleButton),
										new PropertyMetadata(default(Brush)));

		static FlatToggleButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof (FlatToggleButton),
			                                         new FrameworkPropertyMetadata(typeof (FlatToggleButton)));
		}

		/// <summary>
		///     The background of this button when it's hovered by the mouse.
		/// </summary>
		public Brush HoveredBackground
		{
			get { return (Brush)GetValue(HoveredBackgroundProperty); }
			set { SetValue(HoveredBackgroundProperty, value); }
		}

		/// <summary>
		///     The foreground color, used when <see cref="IsInverted" /> is set to true.
		/// </summary>
		public Brush InvertedForeground
		{
			get { return (Brush) GetValue(InvertedForegroundProperty); }
			set { SetValue(InvertedForegroundProperty, value); }
		}

		/// <summary>
		///     Whether or not the colors of this toggle button are inverted.
		///     Set to false by default.
		/// </summary>
		/// <remarks>
		///     When inverted, the chevron is drawn in white instead of black.
		/// </remarks>
		public bool IsInverted
		{
			get { return (bool) GetValue(IsInvertedProperty); }
			set { SetValue(IsInvertedProperty, value); }
		}
	}
}
﻿using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;
using FluentAssertions;
using Metrolib.Controls;
using NUnit.Framework;

namespace Metrolib.Test
{
	[TestFixture]
	[RequiresThread(ApartmentState.STA)]
	public sealed class FilterTextBoxTest
	{
		[SetUp]
		public void SetUp()
		{
			_control = new FilterTextBox
				{
					Style = (Style) App.Instance.FindResource(typeof (FilterTextBox))
				};
			_control.ApplyTemplate().Should().BeTrue();
		}

		private FilterTextBox _control;

		public static void ExecuteEvents()
		{
			var frame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
			                                         new DispatcherOperationCallback(ExitFrame), frame);
			Dispatcher.PushFrame(frame);
		}

		public static object ExitFrame(object f)
		{
			((DispatcherFrame) f).Continue = false;
			return null;
		}

		[Test]
		public void TestConstruction()
		{
			_control.AcceptsReturn.Should().BeFalse("Because FilterTextBox must behave like an ordinary TextBox and thus the AcceptsReturn property must default to false");
			_control.AcceptsTab.Should().BeFalse("Because FilterTextBox must behave like an ordinary TextBox and thus the AcceptsTab property must default to false");
		}

		[Test]
		public void TestRemoveFilterText1()
		{
			_control.FilterText = null;

			var peer = new ButtonAutomationPeer(_control.RemoveFilterTextButton);
			var invokeProv = (IInvokeProvider) peer.GetPattern(PatternInterface.Invoke);
			invokeProv.Invoke();

			_control.FilterText.Should().BeNull();
		}

		[Test]
		public void TestRemoveFilterText2()
		{
			_control.FilterText = "Foobar";

			var peer = new ButtonAutomationPeer(_control.RemoveFilterTextButton);
			var invokeProv = (IInvokeProvider) peer.GetPattern(PatternInterface.Invoke);
			invokeProv.Invoke();
			ExecuteEvents();

			_control.FilterText.Should().BeNull();
		}
	}
}
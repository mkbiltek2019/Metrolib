﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScreenshotCreator
{
	/// <summary>
	///     Responsible for providing the documentation for a particular control.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class ControlDocumentationCreator<T>
		: IControlDocumentationCreator<T>
		where T : FrameworkElement, new()
	{
		private readonly Dispatcher _dispatcher;
		private readonly ResourceDictionary _resourceDictionary;
		private readonly Dictionary<string, BitmapSource> _snapshots;
		private readonly ControlDocumentationWriter<T> _documentationWriter;

		public ControlDocumentationCreator(Dispatcher dispatcher,
		                                   ResourceDictionary resourceDictionary,
		                                   AssemblyDocumentationReader assemblyDocumentationReader)
		{
			if (dispatcher == null)
				throw new ArgumentNullException(nameof(dispatcher));
			if (resourceDictionary == null)
				throw new ArgumentNullException(nameof(resourceDictionary));
			if (assemblyDocumentationReader == null)
				throw new ArgumentNullException(nameof(assemblyDocumentationReader));

			_dispatcher = dispatcher;
			_resourceDictionary = resourceDictionary;
			_snapshots = new Dictionary<string, BitmapSource>();
			_documentationWriter = new ControlDocumentationWriter<T>(assemblyDocumentationReader);
		}

		public void SaveAllPoses(string basePath)
		{
			var elementName = typeof(T).Name;

			foreach (var pair in _snapshots)
			{
				var relativeImagePath = pair.Key;
				var bitmap = pair.Value;

				var destination = Path.Combine(basePath, relativeImagePath);
				SaveSnapshot(bitmap, destination);
			}

			var dest = Path.Combine(basePath, elementName, "README.md");
			using (var fileStream = File.Open(dest, FileMode.Create))
			using (var streamWriter = new StreamWriter(fileStream))
			{
				_documentationWriter.WriteTo(streamWriter);
			}
		}

		[Pure]
		public IControlExampleCreator<T> AddExample(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			var exampleWriter = _documentationWriter.AddExample(name);
			return new ControlExampleCreator<T>(this, _dispatcher, _resourceDictionary, exampleWriter, name);
		}

		internal string AddImage(BitmapSource screenshot, string name)
		{
			var controlName = typeof(T).Name;
			var relativeImagePath = Path.Combine(controlName, string.Format("{0}.png", name));
			_snapshots.Add(relativeImagePath, screenshot);
			return relativeImagePath;
		}

		private static void SaveSnapshot(BitmapSource screenshot, string destination)
		{
			var encoder = new PngBitmapEncoder();
			using (var stream = new MemoryStream())
			{
				var frame = BitmapFrame.Create(screenshot);
				encoder.Frames.Add(frame);
				encoder.Save(stream);

				stream.Position = 0;

				var destinationDirectory = Path.GetDirectoryName(destination);
				Directory.CreateDirectory(destinationDirectory);
				using (var fileStream = File.OpenWrite(destination))
				{
					stream.CopyTo(fileStream);
				}
			}
		}
	}
}
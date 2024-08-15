﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Extensions.Reactive.Bindings;
using Uno.Extensions.Reactive.Config;
using Uno.Extensions.Reactive.Core.HotReload;
using Uno.Extensions.Reactive.Testing;

namespace Uno.Extensions.Reactive.Tests.IntegrationTests;

[TestClass]
public partial class Given_HotReload : FeedUITests
{
	/// <inheritdoc />
	[TestInitialize]
	public override void Initialize()
	{
		FeedConfiguration.HotReload = HotReloadSupport.Enabled;
		typeof(FeedConfiguration).GetField("_effectiveHotReload", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, null);

		Console.WriteLine("Hot reload configuration is now : " + FeedConfiguration.EffectiveHotReload);

		base.Initialize();
	}

	/// <inheritdoc />
	[TestCleanup]
	public override void Cleanup()
	{
		FeedConfiguration.HotReload = null;
		typeof(FeedConfiguration).GetField("_effectiveHotReload", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, null);

		Console.WriteLine("Hot reload configuration has been restored to : " + FeedConfiguration.EffectiveHotReload);

		base.Cleanup();
	}

	#region When_UpdateValueTypeFeedInModel_Then_BindableUpdated
	[TestMethod]
	public async Task When_UpdateValueTypeFeedInModel_Then_BindableUpdated()
	{
		var bindable = new When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyViewModel();

		var tcs = new TaskCompletionSource();
		Dispatcher.TryEnqueue(() => bindable.PropertyChanged += (s, e) => tcs.TrySetResult());

		await WaitFor(() => bindable.MyFeed, "Feed value from original model");

		HotReloadService.UpdateApplication(new[] { typeof(When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyModel_v1) });

		await tcs.Task;
		await WaitFor(() => bindable.MyFeed, "Feed value from model v1");
	}

	[ReactiveBindable(true)]
	public partial class When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyModel
	{
		public IFeed<string> MyFeed => Feed.Async(async ct => "Feed value from original model");
	}

	[ReactiveBindable(false)]
	[Model(bindable: typeof(When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyViewModel))]
	[MetadataUpdateOriginalType(typeof(When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyModel))]
	public partial class When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyModel_v1
	{
		internal When_UpdateValueTypeFeedInModel_Then_BindableUpdated_MyViewModel __reactiveBindableViewModel = default!;

		public IFeed<string> MyFeed => Feed.Async(async ct => "Feed value from model v1");
	}
	#endregion

	#region When_UpdateRecordFeedInModel_Then_BindableUpdated
	[TestMethod]
	public async Task When_UpdateRecordFeedInModel_Then_BindableUpdated()
	{
		var bindable = new When_UpdateRecordFeedInModel_Then_BindableUpdated_MyViewModel();

		var tcs = new TaskCompletionSource();
		Dispatcher.TryEnqueue(() => bindable.PropertyChanged += (s, e) => tcs.TrySetResult());

		await WaitFor(() => bindable.MyFeed.Value, "Feed value from original model");

		HotReloadService.UpdateApplication(new[] { typeof(When_UpdateRecordFeedInModel_Then_BindableUpdated_MyModel_v1) });

		await tcs.Task;
		await WaitFor(() => bindable.MyFeed.Value, "Feed value from model v1");
	}

	public record When_UpdateRecordFeedInModel_Then_BindableUpdated_Record(string Value);

	[ReactiveBindable(true)]
	public partial class When_UpdateRecordFeedInModel_Then_BindableUpdated_MyModel
	{
		public IFeed<When_UpdateRecordFeedInModel_Then_BindableUpdated_Record> MyFeed => Feed.Async(async ct => new When_UpdateRecordFeedInModel_Then_BindableUpdated_Record("Feed value from original model"));
	}

	[ReactiveBindable(false)]
	[Model(bindable: typeof(When_UpdateRecordFeedInModel_Then_BindableUpdated_MyViewModel))]
	[MetadataUpdateOriginalType(typeof(When_UpdateRecordFeedInModel_Then_BindableUpdated_MyModel))]
	public partial class When_UpdateRecordFeedInModel_Then_BindableUpdated_MyModel_v1
	{
		internal When_UpdateRecordFeedInModel_Then_BindableUpdated_MyViewModel __reactiveBindableViewModel = default!;

		public IFeed<When_UpdateRecordFeedInModel_Then_BindableUpdated_Record> MyFeed => Feed.Async(async ct => new When_UpdateRecordFeedInModel_Then_BindableUpdated_Record("Feed value from model v1"));
	}
	#endregion

	#region When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated
	[TestMethod]
	public async Task When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated()
	{
		var bindable = new When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyViewModel();

		var tcs = new TaskCompletionSource();
		Dispatcher.TryEnqueue(() => bindable.PropertyChanged += (s, e) => tcs.TrySetResult());

		await WaitFor(() => bindable.MyFeed, "Feed value from original model");

		HotReloadService.UpdateApplication(new[] { typeof(When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyModel_v1) });

		await tcs.Task;
		await WaitFor(() => bindable.MyFeed, "Feed value from model v1");
	}

	[ReactiveBindable(true)]
	public partial class When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyModel
	{
		public IFeed<string> MyFeed => Feed.Async(async ct => "Feed value from original model");
	}

	[ReactiveBindable(false)]
	[Model(bindable: typeof(When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyViewModel))]
	[MetadataUpdateOriginalType(typeof(When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyModel))]
	public partial class When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyModel_v1
	{
		internal When_ChangeKindOfValueTypeFeedInModel_Then_BindableUpdated_MyViewModel __reactiveBindableViewModel = default!;

		public IFeed<string> MyFeed => Feed.Dynamic(async ct => "Feed value from model v1");
	}
	#endregion

	#region When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated
	[TestMethod]
	public async Task When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated()
	{
		var bindable = new When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyViewModel();

		var tcs = new TaskCompletionSource();
		Dispatcher.TryEnqueue(() => bindable.PropertyChanged += (s, e) => tcs.TrySetResult());

		await WaitFor(() => bindable.MyFeed.Value, "Feed value from original model");

		HotReloadService.UpdateApplication(new[] { typeof(When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyModel_v1) });

		await tcs.Task;
		await WaitFor(() => bindable.MyFeed.Value, "Feed value from model v1");
	}

	public record When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_Record(string Value);

	[ReactiveBindable(true)]
	public partial class When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyModel
	{
		public IFeed<When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_Record> MyFeed => Feed.Async(async ct => new When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_Record("Feed value from original model"));
	}

	[ReactiveBindable(false)]
	[Model(bindable: typeof(When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyViewModel))]
	[MetadataUpdateOriginalType(typeof(When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyModel))]
	public partial class When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyModel_v1
	{
		internal When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_MyViewModel __reactiveBindableViewModel = default!;

		public IFeed<When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_Record> MyFeed => Feed.Dynamic(async ct => new When_ChangeKindOfRecordFeedInModel_Then_BindableUpdated_Record("Feed value from model v1"));
	}
	#endregion

	private async Task WaitFor(Func<bool> predicate)
	{
		for (var i = 0; i < 100; i++)
		{
			if (predicate())
			{
				return;
			}

			await Task.Delay(1);
		}

		throw new TimeoutException();
	}

	private async Task WaitFor<T>(Func<T> actual, T expected)
		where T : class
	{
		const int attempts = 100;
		for (var i = 0; i < attempts; i++)
		{
			if (actual() == expected)
			{
				return;
			}

			await Task.Delay(3);
		}

		throw new TimeoutException($"Expected '{expected}' but get '{actual()}' after {attempts}ms.");
	}
}

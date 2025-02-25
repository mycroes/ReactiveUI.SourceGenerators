﻿// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.SourceGenerators;

namespace SGReactiveUI.SourceGenerators.Test.Maui
{
    /// <summary>
    /// IViewForTest.
    /// </summary>
    /// <seealso cref="NavigationPage" />
    [IViewFor(nameof(TestViewModel))]
    public partial class IViewForTest : Shell;
}

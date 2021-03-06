﻿//----------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Cache;

namespace Test.MSAL.NET.Unit.Mocks
{
    internal class TokenCacheHelper
    {
        public static long ValidExpiresIn = 28800;
        
        public static void PopulateCache(TokenCachePlugin cachePlugin)
        {
            TokenCacheItem item = new TokenCacheItem()
            {
                Authority = TestConstants.AuthorityHomeTenant,
                ClientId = TestConstants.ClientId,
                Policy = TestConstants.Policy,
                TokenType = "Bearer",
                ExpiresOnUnixTimestamp = MsalHelpers.DateTimeToUnixTimestamp(new DateTimeOffset(DateTime.UtcNow + TimeSpan.FromSeconds(ValidExpiresIn))),
                RawIdToken = MockHelpers.DefaultIdToken,
                User = new User
                {
                    DisplayableId = TestConstants.DisplayableId,
                    UniqueId = TestConstants.UniqueId,
                    HomeObjectId = TestConstants.HomeObjectId
                },
                Scope = TestConstants.Scope
            };
            item.Token = item.GetTokenCacheKey().ToString();
            //add access token
            cachePlugin.TokenCacheDictionary[item.GetTokenCacheKey().ToString()] = JsonHelper.SerializeToJson(item);

            item = new TokenCacheItem()
            {
                Authority = TestConstants.AuthorityGuestTenant,
                ClientId = TestConstants.ClientId,
                Policy = TestConstants.Policy,
                TokenType = "Bearer",
                ExpiresOnUnixTimestamp = MsalHelpers.DateTimeToUnixTimestamp(new DateTimeOffset(DateTime.UtcNow + TimeSpan.FromSeconds(ValidExpiresIn))),
                RawIdToken = MockHelpers.CreateIdToken(TestConstants.UniqueId + "more", TestConstants.DisplayableId, TestConstants.HomeObjectId),
                User = new User
                {
                    DisplayableId = TestConstants.DisplayableId,
                    UniqueId = TestConstants.UniqueId + "more",
                    HomeObjectId = TestConstants.HomeObjectId
                },
                Scope = TestConstants.ScopeForAnotherResource
            };
            item.Token = item.GetTokenCacheKey().ToString();
            //add another access token
            cachePlugin.TokenCacheDictionary[item.GetTokenCacheKey().ToString()] = JsonHelper.SerializeToJson(item);
            
            RefreshTokenCacheItem rtItem = new RefreshTokenCacheItem()
            {
                Authority = TestConstants.AuthorityHomeTenant,
                ClientId = TestConstants.ClientId,
                Policy = TestConstants.Policy,
                RefreshToken = "someRT",
                RawIdToken = MockHelpers.DefaultIdToken,
                User = new User
                {
                    DisplayableId = TestConstants.DisplayableId,
                    UniqueId = TestConstants.UniqueId,
                    HomeObjectId = TestConstants.HomeObjectId
                }
            };
            cachePlugin.TokenCacheDictionary[rtItem.GetTokenCacheKey().ToString()] = JsonHelper.SerializeToJson(rtItem);
        }
    }
}

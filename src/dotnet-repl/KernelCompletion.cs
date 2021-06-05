﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Events;
using Pocket;
using RadLine;
using static Pocket.Logger<dotnet_repl.KernelCompletion>;

namespace dotnet_repl
{
    public class KernelCompletion
    {
        private readonly Kernel _kernel;

        public KernelCompletion(Kernel kernel)
        {
            _kernel = kernel;
        }

        public IEnumerable<string> GetCompletions(LineBuffer buffer)
        {
            return GetCompletionsAsync(buffer).Result;
        }

        private async Task<IEnumerable<string>> GetCompletionsAsync(LineBuffer buffer)
        {
            var command = new RequestCompletions(
                buffer.Content,
                new LinePosition(0, buffer.CursorPosition));

            var result = await _kernel.SendAsync(command);

            var completionsProduced = await result
                                          .KernelEvents
                                          .OfType<CompletionsProduced>()
                                          .FirstOrDefaultAsync();

            var code = buffer.Content[..buffer.CursorPosition];

            var matches = completionsProduced
                              ?.Completions
                              .Where(c => c.InsertText.StartsWith(code.Split('.', ' ').LastOrDefault() ?? code))
                              .Select(c => c.InsertText)
                              .ToArray()
                          ?? Array.Empty<string>();

            Log.Info(
                "buffer: {buffer}, code: {code}, matches: {matches}",
                buffer.Content,
                code,
                string.Join(",", matches));

            return matches;
        }
    }
}
﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace RestClientVS.Completion
{
    public abstract class CompletionCommitManagerBase : IAsyncCompletionCommitManagerProvider
    {
        public abstract IEnumerable<char> CommitChars { get; }

        public IAsyncCompletionCommitManager GetOrCreate(ITextView textView) =>
            textView.Properties.GetOrCreateSingletonProperty(() => new DefaultCompletionCommitManager(CommitChars));
    }

    /// <summary>
    /// The simplest implementation of IAsyncCompletionCommitManager that provides Commit Characters and uses default behavior otherwise
    /// </summary>
    public class DefaultCompletionCommitManager : IAsyncCompletionCommitManager
    {
        public DefaultCompletionCommitManager(IEnumerable<char> commitChars)
        {
            _commitChars = commitChars.ToImmutableArray();
        }

        private ImmutableArray<char> _commitChars;

        public IEnumerable<char> PotentialCommitCharacters => _commitChars;

        public virtual bool ShouldCommitCompletion(IAsyncCompletionSession session, SnapshotPoint location, char typedChar, CancellationToken token)
        {
            // This method runs synchronously, potentially before CompletionItem has been computed.
            // The purpose of this method is to filter out characters not applicable at given location.

            // This method is called only when typedChar is among the PotentialCommitCharacters
            // in this simple example, all PotentialCommitCharacters do commit, so we always return true
            return true;
        }

        public virtual CommitResult TryCommit(IAsyncCompletionSession session, ITextBuffer buffer, CompletionItem item, char typedChar, CancellationToken token)
        {
            // Objects of interest here are session.TextView and session.TextView.Caret.
            // This method runs synchronously

            return CommitResult.Unhandled; // use default commit mechanism.
        }
    }
}

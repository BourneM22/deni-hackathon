using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Exceptions
{
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException() { }

        public EmailNotFoundException(string message) : base(message) { }

        public EmailNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException() { }

        public PasswordNotMatchException(string message) : base(message) { }

        public PasswordNotMatchException(string message, Exception inner) : base(message, inner) { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class ImageExtensionException : Exception
    {
        public ImageExtensionException() { }

        public ImageExtensionException(string message) : base(message) { }

        public ImageExtensionException(string message, Exception inner) : base(message, inner) { }
    }

    public class FileSizeExceedException : Exception
    {
        public FileSizeExceedException() { }

        public FileSizeExceedException(string message) : base(message) { }

        public FileSizeExceedException(string message, Exception inner) : base(message, inner) { }
    }

    public class EmptyFileNameException : Exception
    {
        public EmptyFileNameException() { }

        public EmptyFileNameException(string message) : base(message) { }

        public EmptyFileNameException(string message, Exception inner) : base(message, inner) { }
    }

    public class FileNotFoundException : Exception
    {
        public FileNotFoundException() { }

        public FileNotFoundException(string message) : base(message) { }

        public FileNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class EmailAlreadyExistException : Exception
    {
        public EmailAlreadyExistException() { }

        public EmailAlreadyExistException(string message) : base(message) { }

        public EmailAlreadyExistException(string message, Exception inner) : base(message, inner) { }
    }

    public class ReminderNotFoundException : Exception
    {
        public ReminderNotFoundException() { }

        public ReminderNotFoundException(string message) : base(message) { }

        public ReminderNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException() { }

        public NoteNotFoundException(string message) : base(message) { }

        public NoteNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class PriorityIdNotExistException : Exception
    {
        public PriorityIdNotExistException() { }

        public PriorityIdNotExistException(string message) : base(message) { }

        public PriorityIdNotExistException(string message, Exception inner) : base(message, inner) { }
    }

    public class SoundboardDoesNotExistException : Exception
    {
        public SoundboardDoesNotExistException() { }

        public SoundboardDoesNotExistException(string message) : base(message) { }

        public SoundboardDoesNotExistException(string message, Exception inner) : base(message, inner) { }
    }

    public class DoneStatusCannotEmptyException : Exception
    {
        public DoneStatusCannotEmptyException() { }

        public DoneStatusCannotEmptyException(string message) : base(message) { }

        public DoneStatusCannotEmptyException(string message, Exception inner) : base(message, inner) { }
    }

    public class StartTimeExceedEndTimeException : Exception
    {
        public StartTimeExceedEndTimeException() { }

        public StartTimeExceedEndTimeException(string message) : base(message) { }

        public StartTimeExceedEndTimeException(string message, Exception inner) : base(message, inner) { }
    }

    public class FilterIdNotFoundException : Exception
    {
        public FilterIdNotFoundException() { }

        public FilterIdNotFoundException(string message) : base(message) { }

        public FilterIdNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class TagIdNotFoundException : Exception
    {
        public TagIdNotFoundException() { }

        public TagIdNotFoundException(string message) : base(message) { }

        public TagIdNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
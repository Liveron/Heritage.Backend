﻿namespace Heritage.Application.Common.Exceptions;

public abstract class BadRequestException(string message) 
    : Exception(message) { }

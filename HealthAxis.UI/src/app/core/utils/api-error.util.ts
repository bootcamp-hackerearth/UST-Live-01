type ErrorDictionary = Record<string, unknown>;

interface FriendlyApiError {
  friendlyMessage?: unknown;
  error?: unknown;
  message?: unknown;
  title?: unknown;
}

interface ValidationProblemDetails {
  errors?: Record<string, unknown>;
  message?: unknown;
  title?: unknown;
}

export function getFriendlyErrorMessage(
  error: unknown,
  fallbackMessage: string
): string {
  const friendlyMessage = getFriendlyMessage(error);

  if (friendlyMessage) {
    return friendlyMessage;
  }

  const validationMessage = getValidationMessage(error);

  if (validationMessage) {
    return validationMessage;
  }

  const apiMessage = getApiMessage(error);

  if (apiMessage) {
    return apiMessage;
  }

  return fallbackMessage;
}

function getFriendlyMessage(error: unknown): string {
  if (!isRecord(error)) {
    return '';
  }

  const apiError = error as FriendlyApiError;

  if (typeof apiError.friendlyMessage === 'string') {
    return apiError.friendlyMessage;
  }

  return '';
}

function getValidationMessage(error: unknown): string {
  const errorBody = getErrorBody(error);

  if (!isRecord(errorBody)) {
    return '';
  }

  const problemDetails = errorBody as ValidationProblemDetails;

  if (!isRecord(problemDetails.errors)) {
    return '';
  }

  const messages = Object.values(problemDetails.errors)
    .flatMap((value) => Array.isArray(value) ? value : [])
    .filter((value): value is string => typeof value === 'string');

  return messages.join(' ');
}

function getApiMessage(error: unknown): string {
  const errorBody = getErrorBody(error);

  if (!isRecord(errorBody)) {
    return '';
  }

  const possibleMessage = errorBody['message'];
  const possibleTitle = errorBody['title'];

  if (typeof possibleMessage === 'string') {
    return possibleMessage;
  }

  if (typeof possibleTitle === 'string') {
    return possibleTitle;
  }

  return '';
}

function getErrorBody(error: unknown): unknown {
  if (!isRecord(error)) {
    return error;
  }

  const apiError = error as FriendlyApiError;

  if (apiError.error) {
    return apiError.error;
  }

  return error;
}

function isRecord(value: unknown): value is ErrorDictionary {
  return typeof value === 'object' && value !== null;
}
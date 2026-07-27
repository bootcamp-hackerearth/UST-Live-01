type ErrorDictionary =
  Record<string, unknown>;

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
  const friendlyMessage =
    getFriendlyMessage(error);

  if (friendlyMessage) {
    return friendlyMessage;
  }

  const validationMessage =
    getValidationMessage(error);

  if (validationMessage) {
    return validationMessage;
  }

  const apiMessage = getApiMessage(error);

  return apiMessage || fallbackMessage;
}

function getFriendlyMessage(
  error: unknown
): string {
  const directMessage =
    getStringProperty(
      error,
      'friendlyMessage'
    );

  if (directMessage) {
    return directMessage;
  }

  return getStringProperty(
    getErrorBody(error),
    'friendlyMessage'
  );
}

function getValidationMessage(
  error: unknown
): string {
  const errorBody = getErrorBody(error);

  if (!isRecord(errorBody)) {
    return '';
  }

  const problemDetails =
    errorBody as ValidationProblemDetails;

  if (!isRecord(problemDetails.errors)) {
    return '';
  }

  const messages = Object
    .values(problemDetails.errors)
    .flatMap(toMessageList)
    .map((message) => message.trim())
    .filter(Boolean);

  return [...new Set(messages)].join(' ');
}

function toMessageList(
  value: unknown
): string[] {
  if (typeof value === 'string') {
    return [value];
  }

  if (!Array.isArray(value)) {
    return [];
  }

  return value.filter(
    (item): item is string =>
      typeof item === 'string'
  );
}

function getApiMessage(
  error: unknown
): string {
  const errorBody = getErrorBody(error);

  if (typeof errorBody === 'string') {
    return normalizeMessage(errorBody);
  }

  if (!isRecord(errorBody)) {
    return '';
  }

  return (
    getStringProperty(errorBody, 'message') ||
    getStringProperty(errorBody, 'title')
  );
}

function getStringProperty(
  value: unknown,
  propertyName: string
): string {
  if (!isRecord(value)) {
    return '';
  }

  const propertyValue = value[propertyName];

  return typeof propertyValue === 'string'
    ? normalizeMessage(propertyValue)
    : '';
}

function normalizeMessage(
  message: string
): string {
  const normalizedMessage = message.trim();

  if (
    !normalizedMessage ||
    normalizedMessage.startsWith('<')
  ) {
    return '';
  }

  return normalizedMessage;
}

function getErrorBody(error: unknown): unknown {
  if (!isRecord(error)) {
    return error;
  }

  const apiError = error as FriendlyApiError;

  return apiError.error ?? error;
}

function isRecord(
  value: unknown
): value is ErrorDictionary {
  return (
    typeof value === 'object' &&
    value !== null
  );
}
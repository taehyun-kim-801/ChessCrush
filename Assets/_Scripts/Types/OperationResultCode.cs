namespace ChessCrush.OperationResultCode
{
    public enum SignUpCode { Etc = -1, Success = 0, DuplicatedParameterException = 409 }
    public enum SignInCode { Etc = -1, Success = 0, BadUnauthorizedException = 401, Blocked = 403 }
    public enum SetNicknameCode { Etc = -1, Success = 0, BadParameterException = 400, DuplicatedParameterException = 409 }
    public enum RequestFriendCode { Etc = -1, Success = 0, ForbiddenException = 403, DuplicatedParameterException = 409, PreconditionFailed = 412 }
    public enum ParticipateCode { }
    public enum SendActionCode { }
}
namespace ChessCrush.OperationResultCode
{
    public enum SignUpCode { Etc = -1, Success = 0, DuplicatedParameterException = 409 }
    public enum SignInCode { Etc = -1, Success = 0, BadUnauthorizedException = 401, Blocked = 403 }
    public enum ParticipateCode { }
    public enum SendActionCode { }
}
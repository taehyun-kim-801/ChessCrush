namespace ChessCrush.OperationResultCode
{
    public enum SignUpCode { Etc = -1, Success = 0, DuplicatedParameterException = 409 }
    public enum SignInCode { Etc = -1, Success, MissingID, WrongPW }
    public enum ParticipateCode { }
    public enum SendActionCode { }
}
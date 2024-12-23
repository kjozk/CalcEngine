namespace CalcEngine.IO
{
    /// <summary>
    /// 数式の構文エラーを表す例外クラス。
    /// </summary>
    public class SyntaxErrorException : Exception
    {
        /// <summary>
        /// SyntaxErrorException クラスのインスタンスを初期化します。
        /// </summary>
        public SyntaxErrorException() { }

        /// <summary>
        /// 指定したエラーメッセージを使用して、SyntaxErrorException クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">エラーメッセージ。</param>
        public SyntaxErrorException(string message) : base(message) { }

        /// <summary>
        /// 指定したエラーメッセージと内部例外を使用して、SyntaxErrorException クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">エラーメッセージ。</param>
        /// <param name="innerException">内部例外。</param>
        public SyntaxErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}

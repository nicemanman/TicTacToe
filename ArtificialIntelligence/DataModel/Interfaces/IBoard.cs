namespace ArtificialIntelligence.DataModel.Interfaces;

/// <summary>
/// Доска для игры в крестики-нолики
/// </summary>
public interface IBoard
{
    /// <summary>
    /// Получить список клеток доски
    /// </summary>
    /// <returns></returns>
    public List<BoardCell> GetCells();
    
    /// <summary>
    /// Пустая ли выбранная клетка
    /// </summary>
    /// <param name="row">Ряд</param>
    /// <param name="col">Столбец</param>
    /// <returns>true - выбранная клетка пуста</returns>
    bool IsEmpty(int row, int col);

    /// <summary>
    /// Получить текущий счет
    /// </summary>
    /// <returns>Текущий счет</returns>
    int GetCurrentScore();

    /// <summary>
    /// Поставить в игровое поле значок игрока
    /// </summary>
    /// <param name="row">Ряд</param>
    /// <param name="col">Столбец</param>
    void SetPlayer(int row, int col);
    
    /// <summary>
    /// Поставить в игровое поле значок оппонента
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void SetOpponent(int row, int col);

    /// <summary>
    /// Убрать значок из игрового поля
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void UndoMove(int row, int col);

    /// <summary>
    /// Есть ли у игрока выигрышные ряды
    /// </summary>
    bool CheckIfPlayerHasRowsSet();

    /// <summary>
    /// Есть ли у оппонента выигрышные ряды
    /// </summary>
    bool CheckIfOpponentHasRowsSet();

    /// <summary>
    /// Есть ли еще свободные клетки, куда можно сделать ход
    /// </summary>
    bool IsPossibleToMakeAMove();
}
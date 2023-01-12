using static System.Console;
CursorVisible = false; //убираем курсор

int xfield = 10; // размер поля по X с учетом границ
int yfield = 10; // размер поля по Y с учетом границ
int gamespeed = 200; // длительность задержки в мс
bool FieldReady = false; // проверка на то, сгенерировалось ли наше поле
int playerx = 1; // координаты игрока ( курсора ) по Х
int playery = 1; // координаты игрока ( курсора ) по У
int c = 0; // просто счетчик


bool[,] destroy = new bool[yfield + 1, xfield + 1];

int[,] field = new int[yfield + 1, xfield + 1]; // объявляем поле
for (int i = 0; i < yfield; i++) field[i, xfield - 1] = -1; // делаем границы. в графической части это будет серый квадрат, а на самом деле это будут -1
for (int i = 0; i < xfield; i++) field[yfield - 1, i] = -1;
for (int i = 0; i < yfield; i++) field[i, 0] = -1;
for (int i = 0; i < xfield; i++) field[0, i] = -1;
Random rnd = new Random();

int col; // переменная, в которой будет храниться цвет шарика
void Colour() // при вызове этого метода, наш цвет будет меняться на тот, на какую клетку мы попали. ( например при прохождении массива field[i,j] = 2, значит запоминаем этот цвет col = field[i,j]; Colour(). после использования этого метода можно выводить и саму ячейку, и она будет нужного цвета
{
    if (col == 0) Console.ForegroundColor = ConsoleColor.Black;
    if (col == 1) Console.ForegroundColor = ConsoleColor.Yellow;
    if (col == 2) Console.ForegroundColor = ConsoleColor.Blue;
    if (col == 3) Console.ForegroundColor = ConsoleColor.Red;
    if (col == 4) Console.ForegroundColor = ConsoleColor.Green;
    if (col == 5) Console.ForegroundColor = ConsoleColor.Magenta;
    if (col == 6) Console.ForegroundColor = ConsoleColor.Cyan;
}
for (int i = 1; i < yfield - 1; i++) // заполняем всё наше пространство случайными числами, которые на самом деле выступают цветами ( игнорируя границы!! )
{
    for (int j = 1; j < xfield - 1; j++) field[i, j] = rnd.Next(1, 6);
}


for (int i = 0; i < yfield; i++) // выводим наше поле вместе с границами
{
    for (int j = 0; j < xfield; j++) if (field[i, j] == -1) // если граница - выводим █
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("█");
        }
        else // иначе - ноль того цвета, номер которого в поле
        {
            col = field[i, j];
            Colour();
            Console.Write('0');
        }
    Console.WriteLine();

}
void Fall() // метод для "приземления" нулей
{
    for (int j = 1; j < xfield - 1; j++)
    {
        for (int u = 0; u < 100; u++) // повторяем 100 раз, чтобы наверняка все нули упали
        {
            for (int i = yfield - 2; i > 0; i--)
            {
                if (field[i + 1, j] == 0) // если под числом есть пустое пространство ( 0 ) - опускаем его на 1 вниз
                {
                    field[i + 1, j] = field[i, j];
                    Draw(i + 1, j); // этот метод описан немного ниже. он просто обновляет в поле ячейку, которую мы вписали в метод
                    field[i, j] = 0;
                    Draw(i, j);
                }
            }
        }
    }
}
void Draw(int ii, int jj) // этот метод обновляет в поле ячейку, которую мы вписали в метод
{
    Console.SetCursorPosition(jj, ii); // ставим туда курсор
    col = field[ii, jj]; // меняем цвет
    Colour();
    Console.Write('0'); // рисуем ноль такого цвета
}


while (FieldReady == false) // повторяем следующие действия до тех пор, пока поле не будет готово для игры ( не будет 2+ одинаковых цветов в ряд, строку и т.д )
{
    for (int i = 1; i < yfield - 1; i++)
    {
        for (int j = 1; j < xfield - 1; j++)
        {
            if (field[i, j] != 0)
            {
                if (field[i, j] == field[i, j + 1] && field[i, j] == field[i, j - 1]) // проверяем 3 в ряд
                {
                    field[i, j] = 0; // уничтожаем центральное число ( превращаем в 0 ), потом боковые. и рисуем их, чтобы увидеть их уничтожение
                    Draw(i, j);
                    field[i, j + 1] = 0;
                    Draw(i, j + 1);
                    field[i, j - 1] = 0;
                    Draw(i, j - 1);
                    Fall(); // "приземляем" поле, чтобы пустые ячейки заполнились теми, что выше


                }
                if (field[i, j] == field[i + 1, j] && field[i, j] == field[i - 1, j]) // проверяем 3 в столбик, алгоритм тот же, что и в строку
                {
                    field[i, j] = 0;
                    Draw(i, j);
                    field[i + 1, j] = 0;
                    Draw(i + 1, j);
                    field[i - 1, j] = 0;
                    Draw(i - 1, j);
                    Fall();
                }
            }
        }
    }


    for (int i = 1; i < yfield - 1; i++) // тут мы проверяем, готово ли наше поле к игре ( нет ли у нас пустого пространства. все 3 в ряд и в столбик уничтожаются выше, поэтому проверять не нужно )
    {
        for (int j = 1; j < xfield - 1; j++)
        {
            if (field[i, j] == 0) c++;
        }
    }
    if (c == 0) FieldReady = true;
    c = 0;



    for (int i = 1; i < yfield - 1; i++) // заполняем образовавшиеся сверху пустые пространства новыми случайными числами и рисуем их. внутри поля они не сгенерируется, так как перед этим мы "приземлили" всё
    {
        for (int j = 1; j < xfield - 1; j++)
        {
            if (field[i, j] == 0) field[i, j] = rnd.Next(1, 6);
            Draw(i, j);
        }
    }
}

bool game = true;

// ВСЁ ЧТО БЫЛО ВЫШЕ, ЭТО ПРОСТО ГЕНЕРАЦИЯ УРОВНЯ. ТАЙМЕР И ОЧКИ ЗАПУСТЯТСЯ ТОЛЬКО ПОСЛЕ ЭТОЙ СТРОКИ

while (game == true)
{

    bool move = false;


    for (int o = 0; o < 100; o++)
    {
        for (int i = 1; i < yfield - 1; i++)
        {
            for (int j = 1; j < xfield - 1; j++)
            {
                if (field[i, j] != 0)
                {
                    if (field[i, j] == field[i, j + 1] && field[i, j] == field[i, j - 1])
                    {
                        destroy[i, j] = true;
                        destroy[i, j + 1] = true;
                        destroy[i, j - 1] = true;
                    }
                    if (field[i, j] == field[i + 1, j] && field[i, j] == field[i - 1, j])
                    {
                        destroy[i, j] = true;
                        destroy[i + 1, j] = true;
                        destroy[i - 1, j] = true;
                    }
                }
            }
        }
        for (int i = 1; i < yfield - 1; i++)
        {
            for (int j = 0; j < xfield - 1; j++) if (destroy[i, j] == true)
                {
                    field[i, j] = 0;
                    Draw(i, j);
                    Thread.Sleep(gamespeed);
                    destroy[i, j] = false;
                }
        }
        Fall();

        for (int i = 1; i < yfield - 1; i++) // тем же алгоритмом заполняем пустые ячейки, которые образовались в последствии "приземления"
        {
            for (int j = 1; j < xfield - 1; j++)
            {
                if (field[i, j] == 0)
                {
                    field[i, j] = rnd.Next(1, 6);
                }
                Draw(i, j);
            }
        }
    }


    while (move == false && game == true) // цикл выполняется до тех пор, пока мы не походим
    {
        Console.SetCursorPosition(playerx, playery); // переходим в последнее место на поле, где был игрок
        CursorVisible = true; // включаем курсор, чтобы было видно, где мы сейчас
        var x = Console.ReadKey(true).Key; // считываем первое нажатие
        switch (x)
        {
            case ConsoleKey.A: if (playerx - 1 > 0) playerx--; break; // A,D,W,S нужны просто для перемещения по полю
            case ConsoleKey.D: if (playerx + 1 < xfield - 1) playerx++; break;
            case ConsoleKey.W: if (playery - 1 > 0) playery--; break;
            case ConsoleKey.S: if (playery + 1 < yfield - 1) playery++; break;
            case ConsoleKey.K: // нажатие К значит, что сейчас мы попытаемся поменять число, на котором мы стоим с другим
                var xx = Console.ReadKey(true).Key; // считываем второе нажатие
                switch (xx)
                {
                    case ConsoleKey.A: // хотим поменять с числом слева
                        if (playerx - 1 > 0) // проверяяем, не выходит ли за границы
                        {
                            int q = field[playery, playerx]; // запоминаем число, на котором мы стоим чтобы его не потерять
                            field[playery, playerx] = field[playery, playerx - 1]; // меняем наше поле на то, что было слева
                            Draw(playery, playerx); // выводим его
                            field[playery, playerx - 1] = q; // меняем поле слева на наше, которое мы запомнили в переменной q
                            Draw(playery, playerx - 1); // выводим
                            move = true; // запоминаем, что мы сделали шаг, чтобы выйти из режима хода
                            CursorVisible = false; // выключаем курсор
                        }
                        break;
                    case ConsoleKey.D:
                        if (playerx + 1 < xfield - 1)
                        {
                            int q = field[playery, playerx];
                            field[playery, playerx] = field[playery, playerx + 1];
                            Draw(playery, playerx);
                            field[playery, playerx + 1] = q;
                            Draw(playery, playerx + 1);
                            move = true;
                            CursorVisible = false;
                        }
                        break;
                    case ConsoleKey.S:
                        if (playery + 1 < yfield - 1)
                        {
                            int q = field[playery, playerx];
                            field[playery, playerx] = field[playery + 1, playerx];
                            Draw(playery, playerx);
                            field[playery + 1, playerx] = q;
                            Draw(playery + 1, playerx);
                            move = true;
                            CursorVisible = false;
                        }
                        break;
                    case ConsoleKey.W:
                        if (playery - 1 > 0)
                        {
                            int q = field[playery, playerx];
                            field[playery, playerx] = field[playery - 1, playerx];
                            Draw(playery, playerx);
                            field[playery - 1, playerx] = q;
                            Draw(playery - 1, playerx);
                            move = true;
                            CursorVisible = false;
                        }
                        break;
                }
                break;
        }
    }
}
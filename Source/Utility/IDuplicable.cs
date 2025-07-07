public interface IDuplicable<T>
{

    void CopyTo(T target); //Moves Variables over to other Class.

    T Duplicate(); //Makes a Copy of this Class.
}
namespace SpeakWithMe.Library.Services.IServices;

public interface IParcelBoxService {
    string Put(object o);

    object Get(string ticket);
}
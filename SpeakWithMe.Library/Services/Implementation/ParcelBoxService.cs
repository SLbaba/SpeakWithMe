﻿using SpeakWithMe.Library.Services.IServices;

namespace SpeakWithMe.Library.Services.Implementation;

public class ParcelBoxService : IParcelBoxService {
    private readonly Dictionary<string, object> _parcelBox = new();

    public string Put(object o) {
        var token = Guid.NewGuid().ToString();
        _parcelBox[token] = o;
        return token;
    }

    public object Get(string ticket) =>
        _parcelBox.TryGetValue(ticket, out object o) ? o : null;
}
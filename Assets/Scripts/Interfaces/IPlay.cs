using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IPlay
{
    bool IsPlaying
    {
        get;
        set;
    }

    void Play();
}

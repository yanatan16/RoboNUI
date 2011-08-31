using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Messaging
{
    public abstract class ProviderConsumer <Provided, Consumed> : Provider<Provided>, IConsumer<Consumed>
    {

        public abstract void Update(Consumed con);
    }
}

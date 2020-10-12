using Microsoft.Xna.Framework.Graphics;


namespace NeoDraw.Core {

    public interface IElement {

        bool Draw(SpriteBatch sb, bool draw, bool update);

    }

}

import { Tour } from "./tour.model";
import { Show } from "./show.model";

export class TourWithShows extends Tour {
    shows: Show[];
}
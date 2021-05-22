import { TourWithEstimatedProfits } from "./tour-with-estimated-profits.model";
import { Show } from "./show.model";

export class TourWithEstimatedProfitsAndShows extends TourWithEstimatedProfits {
    shows: Show[];
}
